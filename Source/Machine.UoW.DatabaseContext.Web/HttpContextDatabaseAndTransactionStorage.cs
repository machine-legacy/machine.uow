using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class HttpContextDatabaseAndTransactionStorage : DatabaseAndTransactionStorageBase
  {
    protected override IDbConnection InternalConnection
    {
      get { return (IDbConnection)HttpContext.Current.Items[typeof(HttpContextDatabaseAndTransactionStorage).FullName + "-Connection"]; }
      set { HttpContext.Current.Items[typeof(HttpContextDatabaseAndTransactionStorage).FullName + "-Connection"] = value; }
    }

    protected override IDbTransaction InternalTransaction
    {
      get { return (IDbTransaction)HttpContext.Current.Items[typeof(HttpContextDatabaseAndTransactionStorage).FullName + "-Transaction"]; }
      set { HttpContext.Current.Items[typeof(HttpContextDatabaseAndTransactionStorage).FullName + "-Transaction"] = value; }
    }
  }
}