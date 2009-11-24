using System;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NullSessionManager : ISessionManager
  {
    public IManagedSession OpenSession()
    {
      return NullManagedSession.Null;
    }

    public IManagedSession OpenSession(string key)
    {
      return OpenSession();
    }

    public ISession CurrentSession()
    {
      throw new InvalidOperationException();
    }
  }

  public class NullManagedSession : IManagedSession
  {
    public static IManagedSession Null = new NullManagedSession();

    public void Delete<T>(T value)
    {
    }

    public IManagedSession Begin()
    {
      return this;
    }

    public void Save<T>(T value)
    {
    }

    public void Rollback()
    {
    }

    public void Commit()
    {
    }

    public void Dispose()
    {
    }
  }
}