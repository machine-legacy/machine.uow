namespace Machine.UoW.NHibernate
{
  public interface ISessionManager
  {
    IManagedSession OpenSession();
  }
}