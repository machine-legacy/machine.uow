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
        if (_connection == null)
        {
          throw new NoDatabaseConnectionException();
        }
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

    public static bool HasConnection
    {
      get { return _connection != null; }
    }
    
    [ThreadStatic]
    static IDbTransaction _transaction;

    public static IDbTransaction Transaction
    {
      get
      {
        if (_transaction == null)
        {
          throw new NoDatabaseTransactionException();
        }
        return _transaction;
      }
      set
      {
        if (_transaction != value)
        {
          if (_transaction != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Transaction when one is already in use!");
          }
        }
        _transaction = value;
      }
    }

    public static bool HasTransaction
    {
      get { return _transaction != null; }
    }
  }
}
