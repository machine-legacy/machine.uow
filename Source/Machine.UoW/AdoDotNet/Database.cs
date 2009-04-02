using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public static class Database
  {
    [ThreadStatic]
    static IDbConnection _connection;

    public static IDbConnection Connection
    {
      get
      {
        return _connection;
      }
      set
      {
        if (_connection != value)
        {
          if (_connection != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Connection when one is already in use!");
          }
        }
        _connection = value;
      }
    }
  }
}
