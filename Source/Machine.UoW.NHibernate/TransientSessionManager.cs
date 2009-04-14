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

    public IManagedSession OpenSession(object key)
    {
      return new ManagedSession(_sessionFactory.OpenSession());
    }
  }
}