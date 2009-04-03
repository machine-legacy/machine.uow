using System;
using System.Collections.Generic;
using System.Threading;

using NHibernate;

using Machine.Core.Utility;
using Machine.UoW.AdoDotNet;
using Machine.UoW.AmbientTransactions;

namespace Machine.UoW.NHibernate
{
  public class ManagedSession : IManagedSession
  {
    readonly global::NHibernate.ITransaction _transaction;
    readonly ManagedConnection _connection;

    public ManagedSession(ISession session)
    {
      _transaction = session.BeginTransaction();
      _connection = new ManagedConnection(session.Connection, false);
      NH.Session = session;
    }

    public void Rollback()
    {
      NH.Session = null;
      _connection.Rollback();
      _transaction.Rollback();
    }

    public void Commit()
    {
      NH.Session = null;
      _connection.Commit();
      _transaction.Commit();
    }

    public void Dispose()
    {
      NH.Session = null;
      _connection.Dispose();
      _transaction.Dispose();
    }
  }

  public class NullManagedSession : IManagedSession
  {
    public static IManagedSession Null = new NullManagedSession();

    public void Rollback()
    {
    }

    public void Commit()
    {
    }

    public void Dispose()
    {
    }
  }

  public interface IManagedSession : IDisposable
  {
    void Rollback();
    void Commit();
  }
  
  public class SingletonSessionManager : KeyedSessionManager
  {
    readonly object _key = new object();

    public SingletonSessionManager(ISessionFactory sessionFactory)
      : base(sessionFactory)
    {
    }

    public override IManagedSession OpenSession(object key)
    {
      return base.OpenSession(_key);
    }
  }

  public class NullSessionManager : ISessionManager
  {
    public IManagedSession OpenSession(object key)
    {
      return NullManagedSession.Null;
    }
  }

  public class KeyedSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;
    readonly ReaderWriterLock _lock = new ReaderWriterLock();
    readonly Dictionary<object, ISession> _sessions = new Dictionary<object, ISession>();

    public KeyedSessionManager(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public virtual IManagedSession OpenSession(object key)
    {
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => !_sessions.ContainsKey(key)))
        {
          _sessions[key] = _sessionFactory.OpenSession();
        }
        return new ManagedSession(_sessions[key]);
      }
    }
  }

  public class TransientSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;

    public TransientSessionManager(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public IManagedSession OpenSession(object key)
    {
      return new ManagedSession(_sessionFactory.OpenSession());
    }
  }

  public class UnitOfWorkScopeSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public UnitOfWorkScopeSessionManager(ISessionFactory sessionFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _sessionFactory = sessionFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IManagedSession OpenSession(object key)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      ISession session = scope.Get(key, () => {
        return _sessionFactory.OpenSession();
      });
      return new ManagedSession(session);
    }
  }

  public class UnitOfWorkSessionManager : UnitOfWorkScopeSessionManager
  {
    public UnitOfWorkSessionManager(ISessionFactory sessionFactory, IUnitOfWorkProvider unitOfWorkProvider)
      : base(sessionFactory, new CurrentUnitOfWorkScopeProvider(unitOfWorkProvider))
    {
    }
  }

  public class AmbientScopeSessionManager : UnitOfWorkScopeSessionManager
  {
    public AmbientScopeSessionManager(ISessionFactory sessionFactory)
      : base(sessionFactory, new AmbientTransactionUnitOfWorkScopeProvider(NullScope.Null, new UnitOfWorkScopeFactory()))
    {
    }
  }
  
  public interface ISessionManager
  {
    IManagedSession OpenSession(object key);
  }
}
