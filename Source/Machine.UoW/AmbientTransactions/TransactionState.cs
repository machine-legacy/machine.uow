using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;

using Machine.Core.Utility;
using System.Linq;

namespace Machine.UoW.AmbientTransactions
{
  public class TransactionState : IDisposable
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TransactionState));
    readonly static ReaderWriterLock _lock = new ReaderWriterLock();
    readonly static Dictionary<Transaction, TransactionState> _map = new Dictionary<Transaction, TransactionState>();
    readonly IDictionary<Type, IDisposable> _state = new Dictionary<Type, IDisposable>();
    readonly Transaction _transaction;

    public TransactionState(Transaction transaction)
    {
      _transaction = transaction;
    }

    public void Dispose()
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
      foreach (IDisposable value in _state.Values.ToArray())
      {
        value.Dispose();
      }
      _transaction.Dispose();
    }

    public void Set<T>() where T : IDisposable
    {
      _state.Remove(typeof(T));
    }

    public void Set<T>(IDisposable value) where T : IDisposable
    {
      _state[typeof(T)] = value;
    }

    public T Get<T>() where T : IDisposable
    {
      if (_state.ContainsKey(typeof(T)))
      {
        return (T)_state[typeof(T)];
      }
      return default(T);
    }

    public static TransactionState ForCurrentTransaction()
    {
      if (!AmbientTransactionHelpers.InAmbientTransaction())
      {
        throw new InvalidOperationException("Ambient transaction scope invalid unless inside transaction!");
      }
      Transaction transaction = Transaction.Current;
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => !_map.ContainsKey(transaction)))
        {
          Transaction clone = transaction.Clone();
          _map[clone] = new TransactionState(clone);
          transaction.TransactionCompleted += Completed;
          TransactionInformation information = clone.TransactionInformation;
          _log.Info("Creating: " + information.LocalIdentifier + "(" + information.DistributedIdentifier + ")");
        }
        return _map[transaction];
      }
    }

    private static void Completed(object sender, TransactionEventArgs e)
    {
      using (RWLock.AsWriter(_lock))
      {
        TransactionState scope = _map[e.Transaction];
        scope.Dispose();
        _map.Remove(e.Transaction);
      }
    }

    public static void ClearFromEveryScope(IUnitOfWorkScope unitOfWorkScope)
    {
      using (RWLock.AsWriter(_lock))
      {
        foreach (TransactionState state in _map.Values)
        {
          if (state.Get<IUnitOfWorkScope>() == unitOfWorkScope)
          {
            state.Set<IUnitOfWorkScope>();
          }
        }
      }
    }
  }
}