using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

using NHibernate;

using Machine.UoW.AdoDotNet;
using NHibernate.Transaction;

namespace Machine.UoW.NHibernate
{
  public class ManagedTransactionSession : IManagedSession
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ManagedTransactionSession));
    readonly ManagedSession _parent;
    readonly ITransaction _transaction;
    readonly ManagedConnection _connection;

    public ManagedTransactionSession(ManagedSession parent, ISession session)
    {
      _log.Debug("Begin");
      _parent = parent;
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
      _log.Debug("Rollback");
      _transaction.Rollback();
    }

    public void Commit()
    {
      _log.Debug("Commit");
      _transaction.Commit();
    }

    public void Dispose()
    {
      _log.Debug("Dispose");
      _transaction.Dispose();
      _connection.Dispose();
      _parent.ClearTransaction();
    }
  }
  
  public class ManagedSession : IManagedSession
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ManagedSession));
    readonly ISession _session;
    readonly bool _shouldDispose;
    ManagedTransactionSession _transaction;
    bool _inFirstTransaction = true;

    public ManagedSession(ISession session, bool shouldDispose)
    {
      _log.Debug("Begin");
      _session = session;
      _shouldDispose = shouldDispose;
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
      _log.Debug("Rollback");
      _transaction.Rollback();
      _transaction.Dispose();
      ClearTransaction();
    }

    public void Commit()
    {
      if (_transaction == null) throw new InvalidOperationException("No transaction");
      _log.Debug("Commit");
      _transaction.Commit();
      _transaction.Dispose();
      ClearTransaction();
    }

    public void Dispose()
    {
      _log.Debug("Dispose");
      NH.Session = null;
      if (_transaction != null)
      {
        _transaction.Dispose();
      }
      if (_shouldDispose)
      {
        _session.Dispose();
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
}
