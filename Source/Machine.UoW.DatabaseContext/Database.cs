using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public static class Database
  {
    static IContextStorage<IDbConnection> _connectionStorage = new ThreadStaticStorage<IDbConnection>();
    static IContextStorage<IDbTransaction> _transactionStorage = new ThreadStaticStorage<IDbTransaction>();

    public static IContextStorage<IDbConnection> ConnectionStorage
    {
      get { return _connectionStorage; }
      set { _connectionStorage = value; }
    }

    public static IContextStorage<IDbTransaction> TransactionStorage
    {
      get { return _transactionStorage; }
      set { _transactionStorage = value; }
    }

    public static IDbConnection Connection
    {
      get { return _connectionStorage.Peek(); }
    }

    public static bool HasConnection
    {
      get { return !_connectionStorage.IsEmpty; }
    }
    
    public static IDbTransaction Transaction
    {
      get { return _transactionStorage.Peek(); }
    }

    public static bool HasTransaction
    {
      get { return !_transactionStorage.IsEmpty; }
    }
  }
}
