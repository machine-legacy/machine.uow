using System;
using System.Collections.Generic;
using System.Threading;

using NHibernate;

using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate
{
  public class ManagedSession : IManagedSession
  {
    readonly global::NHibernate.ITransaction _transaction;
    readonly ManagedConnection _connection;

    public ManagedSession(ISession session)
    {
      _transaction = session.BeginTransaction();
      _connection = new ManagedConnection(session.Connection, false);
      NH.Session = session;
    }

    public void Save<T>(T value)
    {
      NH.Session.Save(value);
    }

    public void Delete<T>(T value)
    {
      NH.Session.Delete(value);
    }

    public void Rollback()
    {
      NH.Session = null;
      _connection.Rollback();
      _transaction.Rollback();
    }

    public void Commit()
    {
      NH.Session = null;
      _connection.Commit();
      _transaction.Commit();
    }

    public void Dispose()
    {
      NH.Session = null;
      _connection.Dispose();
      _transaction.Dispose();
    }
  }

  public interface IManagedSession : IDisposable
  {
    void Save<T>(T value);
    void Delete<T>(T value);
    void Rollback();
    void Commit();
  }

  public interface ISessionManager
  {
    IManagedSession OpenSession(object key);
  }
}
