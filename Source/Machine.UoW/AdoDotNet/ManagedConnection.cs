using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public class ManagedConnection : IManagedConnection
  {
    readonly IDbConnection _connection;
    readonly IDbTransaction _transaction;

    public ManagedConnection(IDbConnection connection, bool transactional)
    {
      _transaction = transactional ? connection.BeginTransaction() : null;
      _connection = connection;
      Database.Connection = _connection;
      Database.Transaction = _transaction;
    }

    public void Rollback()
    {
      Database.Transaction = null;
      Database.Connection = null;
      if (_transaction != null) _transaction.Rollback();
    }

    public void Commit()
    {
      Database.Transaction = null;
      Database.Connection = null;
      if (_transaction != null) _transaction.Commit();
    }

    public void Dispose()
    {
      Database.Transaction = null;
      Database.Connection = null;
      if (_transaction != null) _transaction.Dispose();
    }
  }

  public interface IManagedConnection : IDisposable
  {
    void Rollback();
    void Commit();
  }

  public interface IConnectionManager
  {
    IManagedConnection OpenConnection(object key);
  }
}
