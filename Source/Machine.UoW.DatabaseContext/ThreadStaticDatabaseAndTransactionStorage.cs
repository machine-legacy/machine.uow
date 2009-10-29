using System;
using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public class ThreadStaticDatabaseAndTransactionStorage : DatabaseAndTransactionStorageBase
  {
    [ThreadStatic]
    static IDbConnection _connection;
    [ThreadStatic]
    static IDbTransaction _transaction;

    protected override IDbConnection InternalConnection
    {
      get { return _connection; }
      set { _connection = value; }
    }

    protected override IDbTransaction InternalTransaction
    {
      get { return _transaction; }
      set { _transaction = value; }
    }
  }
}