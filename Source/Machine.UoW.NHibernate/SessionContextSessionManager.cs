using System;
using NHibernate;
using NHibernate.Context;

namespace Machine.UoW.NHibernate
{
  public class SessionContextSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;

    public SessionContextSessionManager(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
      SessionFactories.Add(_sessionFactory);
    }

    public IManagedSession OpenSession()
    {
      if (CurrentSessionContext.HasBind(_sessionFactory))
      {
        return new ManagedSession(_sessionFactory.GetCurrentSession(), false);
      }
      var session = _sessionFactory.OpenSession();
      CurrentSessionContext.Bind(session);
      return new ManagedSession(session, false);
    }
  }
}
