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
  public class SorryAboutThisHackToGetTransactionsFromNH
  {
    readonly static System.Func<ITransaction, IDbTransaction> _extractAdoTransaction;

    static SorryAboutThisHackToGetTransactionsFromNH()
    {
      FieldInfo field = typeof(AdoTransaction).GetField("trans", BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == null) throw new ArgumentException();
      _extractAdoTransaction = (nh) => {
        return (IDbTransaction)field.GetValue(nh);
      };
    }

    public static IDbTransaction GetAdoNetTransaction(ISession session)
    {
      if (session.Transaction == null)
        return null;
      return _extractAdoTransaction(session.Transaction);
    }
  }

  public class ManagedTransactionSession : IManagedSession
  {
    readonly ManagedSession _parent;
    readonly ITransaction _transaction;
    readonly ManagedConnection _connection;

    public ManagedTransactionSession(ManagedSession parent, ISession session)
    {
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
    readonly bool _shouldDispose;
    ManagedTransactionSession _transaction;
    bool _inFirstTransaction = true;

    public ManagedSession(ISession session, bool shouldDispose)
    {
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

  public interface ISessionManager
  {
    IManagedSession OpenSession(object key);
    void DisposeAndRemoveSession(object key);
  }
}
