using System;
using System.Collections.Generic;

using Machine.UoW.AdoDotNet;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NestableSessionManager : IScopedSessionManager
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(NestableSessionManager));
    readonly ISessionManager _sessionManager;
    readonly ISessionFactory _sessionFactory;
    [ThreadStatic]
    static NestedSessionScope _scope;

    public NestableSessionManager(ISessionManager sessionManager, ISessionFactory sessionFactory)
    {
      _sessionManager = sessionManager;
      _sessionFactory = sessionFactory;
    }

    public void Begin()
    {
      _scope = new NestedSessionScope(_sessionManager, _sessionFactory);
    }

    public IManagedSession OpenSession(object key)
    {
      if (_scope != null)
      {
        return _scope.OpenSession();
      }
      if (NH.HasSession)
      {
        return new NullManagedSession();
      }
      return _sessionManager.OpenSession(key);
    }

    public void End()
    {
      if (_scope == null) return;
      _scope.End();
      _scope = null;
    }
  }

  public class NestedSessionScope
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(NestedSessionScope));
    readonly ISessionManager _sessionManager;
    readonly ISessionFactory _sessionFactory;
    ISession _session;

    public NestedSessionScope(ISessionManager sessionManager, ISessionFactory sessionFactory)
    {
      _sessionManager = sessionManager;
      _sessionFactory = sessionFactory;
    }

    public IManagedSession OpenSession()
    {
      if (_session == null)
      {
        _session = _sessionFactory.OpenSession();
        _session.BeginTransaction();
      }
      return new NestedManagedSession(_session);
    }

    public void End()
    {
      if (_session == null) return;
      if (!_session.Transaction.WasRolledBack)
      {
        _session.Transaction.Commit();
      }
      _session.Dispose();
    }
  }

  public class NestedManagedSession : IManagedSession
  {
    readonly ISession _session;
    bool? _vote;

    public NestedManagedSession(ISession session)
    {
      _session = session;
      NH.Session = session;
      Database.Connection = session.Connection;
      Database.Transaction = SorryAboutThisHackToGetTransactionsFromNH.GetAdoNetTransaction(_session);
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
      return new NestedManagedSession(_session);
    }

    public void Rollback()
    {
      _session.Transaction.Rollback();
      _vote = false;
    }

    public void Commit()
    {
      _vote = true;
    }

    public void Dispose()
    {
      NH.Session = null;
      Database.Connection = null;
      Database.Transaction = null;
      if (_vote != null) return;
      _session.Transaction.Rollback();
    }
  }

  public interface IScopedSessionManager
  {
    void Begin();
    IManagedSession OpenSession(object key);
    void End();
  }

  public class NullScopedSessionManager : IScopedSessionManager
  {
    public void Begin()
    {
    }

    public IManagedSession OpenSession(object key)
    {
      return new NullManagedSession();
    }

    public void End()
    {
    }
  }
}
