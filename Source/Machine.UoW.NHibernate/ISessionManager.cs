using NHibernate;

namespace Machine.UoW.NHibernate
{
  public interface ISessionManager
  {
    IManagedSession OpenSession();
    IManagedSession OpenSession(string key);
    ISession CurrentSession();
  }
}