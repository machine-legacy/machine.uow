using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public class UnitOfWorkScopeConnectionManager : IConnectionManager
  {
    readonly IConnectionProvider _connectionProvider;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public UnitOfWorkScopeConnectionManager(IConnectionProvider connectionProvider, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _connectionProvider = connectionProvider;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IManagedConnection OpenConnection(object key)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      IDbConnection connection = scope.Get(key, () => {
                                                        return _connectionProvider.OpenConnection();
      });
      return new ManagedConnection(connection);
    }
  }
}