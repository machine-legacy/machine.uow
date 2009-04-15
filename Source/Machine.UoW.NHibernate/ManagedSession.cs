using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NHibernate;

using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate
{
  public class ManagedTransactionSession : IManagedSession
  {
    readonly ManagedSession _parent;
    readonly ITransaction _transaction;
    readonly ManagedConnection _connection;

    public ManagedTransactionSession(ManagedSession parent, ISession session)
    {
      _parent = parent;
      _transaction = session.BeginTransaction();
      _connection = new ManagedConnection(session.Connection, false);
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
      _transaction.Rollback();
    }

    public void Commit()
    {
      _transaction.Commit();
    }

    public void Dispose()
    {
      _transaction.Dispose();
      _connection.Dispose();
      _parent.ClearTransaction();
    }
  }
  
  public class ManagedSession : IManagedSession
  {
    readonly ISession _session;
    ManagedTransactionSession _transaction;
    bool _inFirstTransaction = true;

    public ManagedSession(ISession session)
    {
      _session = session;
      _transaction = new ManagedTransactionSession(this, session);
      NH.Session = _session;
    }

    public void Save<T>(T value)
    {
      _session.Save(value);
    }

    public void Delete<T>(T value)
    {
      _session.Delete(value);
    }

    public IManagedSession Begin()
    {
      if (_transaction != null)
      {
        if (_inFirstTransaction)
        {
          _inFirstTransaction = false;
          return _transaction;
        }
        throw new InvalidOperationException("can't open multiple transactions");
      }
      _transaction = new ManagedTransactionSession(this, _session);
      return _transaction;
    }

    public void Rollback()
    {
      if (_transaction == null) throw new InvalidOperationException("No transaction");
      _transaction.Rollback();
      _transaction.Dispose();
      ClearTransaction();
    }

    public void Commit()
    {
      if (_transaction == null) throw new InvalidOperationException("No transaction");
      _transaction.Commit();
      _transaction.Dispose();
      ClearTransaction();
    }

    public void Dispose()
    {
      NH.Session = null;
      _session.Dispose();
      if (_transaction != null)
      {
        _transaction.Dispose();
      }
    }

    public void ClearTransaction()
    {
      _transaction = null;
    }
  }

  public interface IManagedSession : IDisposable
  {
    void Save<T>(T value);
    void Delete<T>(T value);
    IManagedSession Begin();
    void Rollback();
    void Commit();
  }

  public interface ISessionManager
  {
    IManagedSession OpenSession(object key);
  }
}
