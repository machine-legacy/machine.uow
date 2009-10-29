using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext
{
  public static class Database
  {
    static IDatabaseAndTransactionStorage _databaseAndTransactionStorage = new ThreadStaticDatabaseAndTransactionStorage();

    public static IDatabaseAndTransactionStorage Storage
    {
      get { return _databaseAndTransactionStorage; }
      set { _databaseAndTransactionStorage = value; }
    }

    public static IDbConnection Connection
    {
      get { return _databaseAndTransactionStorage.Connection; }
      set { _databaseAndTransactionStorage.Connection = value; }
    }

    public static bool HasConnection
    {
      get { return _databaseAndTransactionStorage.HasConnection; }
    }
    
    public static IDbTransaction Transaction
    {
      get { return _databaseAndTransactionStorage.Transaction; }
      set { _databaseAndTransactionStorage.Transaction = value; }
    }

    public static bool HasTransaction
    {
      get { return _databaseAndTransactionStorage.HasTransaction; }
    }
  }
}
