using System;
using System.Collections.Generic;
using System.Data;

using Machine.UoW.AmbientTransactions;

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

  public class NullManagedConnection : IManagedConnection
  {
    public void Dispose()
    {
    }

    public void Commit()
    {
    }
  }

  public interface IManagedConnection : IDisposable
  {
    void Commit();
  }

  public class AmbientScopeConnectionManager : IConnectionManager
  {
    readonly IConnectionProvider _connectionProvider;
    readonly AmbientTransactionUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public AmbientScopeConnectionManager(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
      _unitOfWorkScopeProvider = new AmbientTransactionUnitOfWorkScopeProvider(NullScope.Null, new UnitOfWorkScopeFactory());
    }

    public IManagedConnection OpenConnection(object key)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      IDbConnection connection = scope.Get(key, () => {
        return _connectionProvider.OpenConnection();
      });
      return new ManagedConnection(connection, true);
    }
  }

  public interface IConnectionManager
  {
    IManagedConnection OpenConnection(object key);
  }
}
