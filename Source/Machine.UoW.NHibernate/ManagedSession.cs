using System;
using System.Collections.Generic;

using NHibernate;

namespace Machine.UoW.NHibernate
{
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
      NH.Storage.Push(_session);
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
      if (_session != NH.Storage.Pop())
        throw new InvalidOperationException("Popped session is NOT the one that was pushed?");
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
