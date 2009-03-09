using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Machine.UoW.SqlServer
{
  public class SqlServerConnectionProvider : IConnectionProvider
  {
    readonly string _connectionString;

    public SqlServerConnectionProvider(string connectionString)
    {
      _connectionString = connectionString;
    }

    public IDbConnection OpenConnection()
    {
      SqlConnection connection = new SqlConnection(_connectionString);
      connection.Open();
      return connection;
    }
  }
}
