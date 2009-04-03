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
    public static IManagedConnection Null = new NullManagedConnection();

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

  public class UnitOfWorkScopeConnectionManager : IConnectionManager
  {
    readonly IConnectionProvider _connectionProvider;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public UnitOfWorkScopeConnectionManager(IConnectionProvider connectionProvider, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _connectionProvider = connectionProvider;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
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

  public class UnitOfWorkConnectionManager : UnitOfWorkScopeConnectionManager
  {
    public UnitOfWorkConnectionManager(IConnectionProvider connectionProvider, IUnitOfWorkProvider unitOfWorkProvider)
      : base(connectionProvider, new CurrentUnitOfWorkScopeProvider(unitOfWorkProvider))
    {
    }
  }

  public class AmbientScopeConnectionManager : UnitOfWorkScopeConnectionManager
  {
    public AmbientScopeConnectionManager(IConnectionProvider connectionProvider)
      : base(connectionProvider, new AmbientTransactionUnitOfWorkScopeProvider(NullScope.Null, new UnitOfWorkScopeFactory()))
    {
    }
  }

  public class NullConnectionManager : IConnectionManager
  {
    public IManagedConnection OpenConnection(object key)
    {
      return NullManagedConnection.Null;
    }
  }

  public interface IConnectionManager
  {
    IManagedConnection OpenConnection(object key);
  }
}
