using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public class CurrentConnection
  {
    readonly IConnectionProvider _connectionProvider;
    IDbConnection _connection;

    public CurrentConnection(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    public IDbConnection Connection()
    {
      if (_connection == null)
      {
        _connection = _connectionProvider.OpenConnection();
      }
      return _connection;
    }

    public void Close()
    {
      if (_connection != null)
      {
        _connection.Close();
        _connection = null;
      }
    }
  }
}
