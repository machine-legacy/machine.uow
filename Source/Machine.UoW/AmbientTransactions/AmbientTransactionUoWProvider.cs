using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;

using Machine.Core.Utility;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUoWProvider : IUnitOfWorkProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AmbientTransactionUoWProvider));
    readonly static ReaderWriterLock _lock = new ReaderWriterLock();
    readonly static Dictionary<Transaction, UnitOfWorkScope> _scope = new Dictionary<Transaction, UnitOfWorkScope>();

    public AmbientTransactionUoWProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      UnitOfWorkScope scope = Scope();
      IUnitOfWork unitOfWork = scope.Get<IUnitOfWork>();
      if (unitOfWork == null)
      {
        unitOfWork = _unitOfWorkFactory.StartUnitOfWork(settings);
        scope.Set(unitOfWork);
        _log.Info("Starting UoW");
      }
      AmbientTransactionSettings ambientTransactionSettings = settings.AmbientSettings();
      return new AmbientTransactionUnitOfWorkProxy(unitOfWork, ambientTransactionSettings.ToScope());
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return AmbientTransactionUnitOfWorkProxy.Active;
    }

    private static UnitOfWorkScope Scope()
    {
      if (!AmbientTransactionHelpers.InAmbientTransaction())
      {
        throw new InvalidOperationException("Ambient transaction scope invalid unless inside transaction!");
      }
      Transaction transaction = Transaction.Current;
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => !_scope.ContainsKey(transaction)))
        {
          Transaction clone = transaction.Clone();
          _scope[clone] = new UnitOfWorkScope(clone);
          transaction.TransactionCompleted += Completed;
          _log.Info("Creating: " + clone.TransactionInformation.LocalIdentifier + "(" + clone.TransactionInformation.DistributedIdentifier + ")");
        }
        return _scope[transaction];
      }
    }

    private static void Completed(object sender, TransactionEventArgs e)
    {
      using (RWLock.AsWriter(_lock))
      {
        UnitOfWorkScope scope = _scope[e.Transaction];
        scope.Dispose();
        _scope.Remove(e.Transaction);
      }
    }
  }

  public class UnitOfWorkScope : UnitOfWorkStateBase, IDisposable
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AmbientTransactionUoWProvider));
    readonly Transaction _transaction;

    public UnitOfWorkScope(Transaction transaction)
    {
      _transaction = transaction;
    }

    public void Dispose()
    {
      _log.Info("Disposing: " + _transaction.TransactionInformation.LocalIdentifier + "(" + _transaction.TransactionInformation.DistributedIdentifier + ")");
      Get<IUnitOfWork>().Dispose();
      _transaction.Dispose();
    }
  }
}
