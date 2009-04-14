using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class UnitOfWorkSessionManager : UnitOfWorkScopeSessionManager
  {
    public UnitOfWorkSessionManager(ISessionFactory sessionFactory, IUnitOfWorkProvider unitOfWorkProvider)
      : base(sessionFactory, new CurrentUnitOfWorkScopeProvider(unitOfWorkProvider))
    {
    }
  }
}