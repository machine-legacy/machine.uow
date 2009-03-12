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
      if (!AmbientTransactionHelpers.InAmbientTransaction())
      {
        throw new InvalidOperationException("Ambient transaction scope invalid unless inside transaction!");
      }
      UnitOfWorkScope scope = Scope();
      IUnitOfWork unitOfWork = scope.Get<IUnitOfWork>();
      if (unitOfWork == null)
      {
        unitOfWork = _unitOfWorkFactory.StartUnitOfWork(settings);
        scope.Set(unitOfWork);
        _log.Info("Starting UoW");
        EnlistmentNotifications.Enlist(unitOfWork);
      }
      AmbientTransactionSettings ambientTransactionSettings = settings.AmbientSettings();
      return new AmbientTransactionUnitOfWorkProxy(unitOfWork, ambientTransactionSettings.ToScope());
    }

    public IUnitOfWork GetUnitOfWork()
    {
      if (!AmbientTransactionHelpers.InAmbientTransaction())
      {
        throw new InvalidOperationException("Ambient transaction scope invalid unless inside transaction!");
      }
      return AmbientTransactionUnitOfWorkProxy.Active;
    }

    private static UnitOfWorkScope Scope()
    {
      Transaction transaction = Transaction.Current;
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => !_scope.ContainsKey(transaction)))
        {
          Transaction clone = transaction.Clone();
          _scope[clone] = new UnitOfWorkScope(clone);
          transaction.TransactionCompleted += Completed;
          TransactionInformation information = clone.TransactionInformation;
          _log.Info("Creating: " + information.LocalIdentifier + "(" + information.DistributedIdentifier + ")");
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

  public class UnitOfWorkScope : Machine.UoW.UnitOfWorkScope, IDisposable
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AmbientTransactionUoWProvider));
    readonly Transaction _transaction;

    public UnitOfWorkScope(Transaction transaction)
      : base(new NullScope())
    {
      _transaction = transaction;
    }

    public override void Dispose()
    {
      TransactionInformation information = _transaction.TransactionInformation;
      _log.Info("Disposing: " + information.LocalIdentifier + " (" + information.DistributedIdentifier + ") " + information.Status);
      switch (information.Status)
      {
        case TransactionStatus.Aborted:
          break;
        case TransactionStatus.Active:
          break;
        case TransactionStatus.Committed:
          break;
        case TransactionStatus.InDoubt:
          break;
      }
      Get<IUnitOfWork>().Dispose();
      _transaction.Dispose();
      // base.Dispose();
    }
  }
}
