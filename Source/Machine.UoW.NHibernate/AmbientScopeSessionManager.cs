using Machine.UoW.AmbientTransactions;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class AmbientScopeSessionManager : UnitOfWorkScopeSessionManager
  {
    public AmbientScopeSessionManager(ISessionFactory sessionFactory)
      : base(sessionFactory, new AmbientTransactionUnitOfWorkScopeProvider(NullScope.Null, new UnitOfWorkScopeFactory()))
    {
    }
  }
}