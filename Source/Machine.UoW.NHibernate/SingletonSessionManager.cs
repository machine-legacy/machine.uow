using NHibernate;

namespace Machine.UoW.NHibernate
{
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
}