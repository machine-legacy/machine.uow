using System;
using System.Data;

namespace Machine.UoW.DatabaseContext
{
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
}