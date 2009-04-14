using Machine.UoW.AmbientTransactions;

namespace Machine.UoW.AdoDotNet
{
  public class AmbientScopeConnectionManager : UnitOfWorkScopeConnectionManager
  {
    public AmbientScopeConnectionManager(IConnectionProvider connectionProvider)
      : base(connectionProvider, new AmbientTransactionUnitOfWorkScopeProvider(NullScope.Null, new UnitOfWorkScopeFactory()))
    {
    }
  }
}