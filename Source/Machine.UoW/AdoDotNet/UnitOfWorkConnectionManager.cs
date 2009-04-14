namespace Machine.UoW.AdoDotNet
{
  public class UnitOfWorkConnectionManager : UnitOfWorkScopeConnectionManager
  {
    public UnitOfWorkConnectionManager(IConnectionProvider connectionProvider, IUnitOfWorkProvider unitOfWorkProvider)
      : base(connectionProvider, new CurrentUnitOfWorkScopeProvider(unitOfWorkProvider))
    {
    }
  }
}