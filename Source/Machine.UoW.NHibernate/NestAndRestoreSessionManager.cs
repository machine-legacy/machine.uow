using System;
using System.Collections.Generic;
using System.Data;

using Machine.UoW.AdoDotNet;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NestAndRestoreSessionManager : ISessionManager
  {
    readonly ISessionManager _sessionManager;

    public NestAndRestoreSessionManager(ISessionManager sessionManager)
    {
      _sessionManager = sessionManager;
    }

    public IManagedSession OpenSession(object key)
    {
      IDbConnection previousConnection = null;
      IDbTransaction previousTransaction = null;
      if (Database.HasConnection)
      {
        previousConnection = Database.Connection;
        Database.Connection = null;
        previousTransaction = Database.Transaction;
        Database.Transaction = null;
      }
      ISession previousSession = null;
      if (NH.HasSession)
      {
        previousSession = NH.Session;
        NH.Session = null;
      }
      return new NestAndRestoreManagedSession(_sessionManager.OpenSession(key), previousConnection, previousTransaction, previousSession);
    }

    public void DisposeAndRemoveSession(object key)
    {
      throw new NotImplementedException();
    }
  }

  public class NestAndRestoreManagedSession : IManagedSession
  {
    readonly IManagedSession _session;
    readonly IDbConnection _previousConnection;
    readonly IDbTransaction _previousTransaction;
    readonly ISession _previousSession;

    public NestAndRestoreManagedSession(IManagedSession session, IDbConnection previousConnection, IDbTransaction previousTransaction, ISession previousSession)
    {
      _session = session;
      _previousSession = previousSession;
      _previousTransaction = previousTransaction;
      _previousConnection = previousConnection;
    }

    public void Save<T>(T value)
    {
      _session.Save(value);
    }

    public void Delete<T>(T value)
    {
      _session.Delete(value);
    }

    public IManagedSession Begin()
    {
      return _session.Begin();
    }

    public void Rollback()
    {
      _session.Rollback();
    }

    public void Commit()
    {
      _session.Commit();
    }

    public void Dispose()
    {
      _session.Dispose();
      if (_previousConnection != null) Database.Connection = _previousConnection;
      if (_previousTransaction != null) Database.Transaction = _previousTransaction;
      if (_previousSession != null) NH.Session = _previousSession;
    }
  }
}
