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
      ISession session = _sessionFactory.OpenSession();
      session.FlushMode = settings.FlushMode;
      return new CurrentSession(session);
    }
  }
}
