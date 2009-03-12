using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public class CurrentConnection : IDisposable
  {
    readonly IConnectionProvider _connectionProvider;
    readonly IDbConnection _connection;

    public CurrentConnection(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
      _connection = _connectionProvider.OpenConnection();
    }

    public IDbConnection Connection()
    {
      return _connection;
    }

    public void Dispose()
    {
      _connection.Close();
    }
  }
}
