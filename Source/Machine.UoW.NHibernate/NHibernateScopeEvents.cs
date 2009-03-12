using System;
using System.Collections.Generic;

using Machine.UoW.AdoDotNet;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NHibernateScopeEvents : IScopeEvents
  {
    readonly ISessionFactory _sessionFactory;

    public NHibernateScopeEvents(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public void Start(IUnitOfWorkScope scope)
    {
      scope.Add(typeof(CurrentSession), new CurrentSessionProvider(_sessionFactory));
      scope.Add(typeof(CurrentNhibernateTransaction), new CurrentTransactionProvider());
    }
  }

  public class CurrentSessionProvider : IScopeProvider
  {
    readonly ISessionFactory _sessionFactory;

    public CurrentSessionProvider(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public IDisposable Create(IUnitOfWorkScope scope)
    {
      NHibernateSessionSettings settings = scope.Get(NHibernateSessionSettings.Default);
      ISession session = _sessionFactory.OpenSession(scope.Connection());
      session.FlushMode = settings.FlushMode;
      return new CurrentSession(session);
    }
  }

  public class CurrentTransactionProvider : IScopeProvider
  {
    public IDisposable Create(IUnitOfWorkScope scope)
    {
      NHibernateSessionSettings settings = scope.Get(NHibernateSessionSettings.Default);
      ISession session = scope.Session();
      ITransaction transaction = session.BeginTransaction(settings.IsolationLevel);
      return new CurrentNhibernateTransaction(transaction);
    }
  }
}
