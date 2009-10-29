using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class ThreadStaticAndHttpContextDatabaseAndTransactionStorage : IDatabaseAndTransactionStorage
  {
    public IDbConnection Connection
    {
      get { return this.CurrentStorage.Connection; }
      set { this.CurrentStorage.Connection = value; }
    }

    public bool HasConnection
    {
      get { return this.CurrentStorage.HasConnection; }
    }

    public IDbTransaction Transaction
    {
      get { return this.CurrentStorage.Transaction; }
      set { this.CurrentStorage.Transaction = value; }
    }

    public bool HasTransaction
    {
      get { return this.CurrentStorage.HasTransaction; }
    }

    IDatabaseAndTransactionStorage CurrentStorage
    {
      get
      {
        if (HttpContext.Current == null)
          return _threadStaticAndHttpContextDatabaseAndTransactionStorage;
        return _httpContextDatabaseAndTransactionStorage;
      }
    }

    readonly IDatabaseAndTransactionStorage _threadStaticAndHttpContextDatabaseAndTransactionStorage = new ThreadStaticDatabaseAndTransactionStorage();
    readonly IDatabaseAndTransactionStorage _httpContextDatabaseAndTransactionStorage = new HttpContextDatabaseAndTransactionStorage();
  }
}