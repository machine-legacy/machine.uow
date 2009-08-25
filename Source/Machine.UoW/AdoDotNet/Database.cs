using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Machine.UoW.AdoDotNet
{
  public static class Database
  {
    static IDatabaseAndTransactionStorage _databaseAndTransactionStorage = new ThreadStaticAndHttpContextDatabaseAndTransactionStorage();

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

  public interface IDatabaseAndTransactionStorage
  {
    IDbConnection Connection { get; set;}
    bool HasConnection { get; }
    IDbTransaction Transaction { get; set; }
    bool HasTransaction { get; }
  }

  public abstract class DatabaseAndTransactionStorageBase : IDatabaseAndTransactionStorage
  {
    protected abstract IDbConnection InternalConnection { get; set; }
    protected abstract IDbTransaction InternalTransaction { get; set; }

    public IDbConnection Connection
    {
      get
      {
        if (InternalConnection == null)
        {
          throw new NoDatabaseConnectionException();
        }
        return InternalConnection;
      }
      set
      {
        if (InternalConnection != value)
        {
          if (InternalConnection != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Connection when one is already in use!");
          }
        }
        InternalConnection = value;
      }
    }

    public bool HasConnection
    {
      get { return InternalConnection != null; }
    }

    public IDbTransaction Transaction
    {
      get
      {
        if (InternalTransaction == null)
        {
          throw new NoDatabaseTransactionException();
        }
        return InternalTransaction;
      }
      set
      {
        if (InternalTransaction != value)
        {
          if (InternalTransaction != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Transaction when one is already in use!");
          }
        }
        InternalTransaction = value;
      }
    }

    public bool HasTransaction
    {
      get { return InternalTransaction != null; }
    }
  }

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
