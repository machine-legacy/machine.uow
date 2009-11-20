using System;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class TransientSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;

    public TransientSessionManager(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public IManagedSession OpenSession()
    {
      return OpenSession(String.Empty);
    }

    public IManagedSession OpenSession(object key)
    {
      return new ManagedSession(_sessionFactory.OpenSession(), true);
    }

    public void DisposeAndRemoveSession(object key)
    {
      throw new InvalidOperationException("Cannot dispose and remove from TransientSessionManager");
    }
  }
}