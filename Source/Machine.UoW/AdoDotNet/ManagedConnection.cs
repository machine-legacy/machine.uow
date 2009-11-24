using System;
using System.Collections.Generic;
using System.Data;
using Machine.UoW.DatabaseContext;

namespace Machine.UoW.AdoDotNet
{
  public class ManagedConnection : IManagedConnection
  {
    readonly IDbConnection _connection;
    readonly IDbTransaction _transaction;

    public ManagedConnection(IDbConnection connection, IDbTransaction transaction)
    {
      _connection = connection;
      _transaction = transaction;
      Database.ConnectionStorage.Push(_connection);
      Database.TransactionStorage.Push(_transaction);
    }

    public ManagedConnection(IDbConnection connection)
    {
      _transaction = connection.BeginTransaction();
      _connection = connection;
      Database.ConnectionStorage.Push(_connection);
      Database.TransactionStorage.Push(_transaction);
    }

    public void Rollback()
    {
      if (_transaction != null) _transaction.Rollback();
    }

    public void Commit()
    {
      if (_transaction != null) _transaction.Commit();
    }

    public void Dispose()
    {
      Database.ConnectionStorage.Pop();
      Database.TransactionStorage.Pop();
      if (_transaction != null) _transaction.Dispose();
    }
  }

  public interface IManagedConnection : IDisposable
  {
    void Rollback();
    void Commit();
  }
}
