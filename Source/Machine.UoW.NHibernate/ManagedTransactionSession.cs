using System;
using Machine.UoW.AdoDotNet;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class ManagedTransactionSession : IManagedSession
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ManagedTransactionSession));
    readonly ManagedSession _parent;
    readonly ITransaction _transaction;
    readonly ManagedConnection _connection;
    readonly bool _transactionOwner;

    public ManagedTransactionSession(ManagedSession parent, ISession session)
    {
      _parent = parent;
      _transactionOwner = !session.Transaction.IsActive;
      _transaction = session.BeginTransaction();
      _connection = new ManagedConnection(session.Connection, SorryAboutThisHackToGetTransactionsFromNH.GetAdoNetTransaction(session));
    }

    public void Save<T>(T value)
    {
      _parent.Save(value);
    }

    public void Delete<T>(T value)
    {
      _parent.Delete(value);
    }

    public IManagedSession Begin()
    {
      throw new InvalidOperationException();
    }

    public void Rollback()
    {
      if (_transaction.WasCommitted || _transaction.WasRolledBack)
        return;
      _transaction.Rollback();
    }

    public void Commit()
    {
      if (!_transactionOwner)
      {
        return;
      }
      if (_transaction.WasRolledBack)
      {
        return;
      }
      _transaction.Commit();
    }

    public void Dispose()
    {
      if (_transactionOwner)
      {
        _transaction.Dispose();
      }
      _connection.Dispose();
      _parent.ClearTransaction();
    }
  }
}