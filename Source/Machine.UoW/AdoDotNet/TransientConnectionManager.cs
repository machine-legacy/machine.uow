namespace Machine.UoW.AdoDotNet
{
  public class TransientConnectionManager : IConnectionManager
  {
    readonly IConnectionProvider _connectionProvider;

    public TransientConnectionManager(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    public IManagedConnection OpenConnection(object key)
    {
      return new ManagedConnection(_connectionProvider.OpenConnection());
    }
  }
}