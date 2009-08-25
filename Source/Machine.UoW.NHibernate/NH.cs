using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class NH
  {
    static ISessionStorage _sessionStorage = new ThreadStaticAndHttpContextSessionStorage();

    public static ISessionStorage Storage
    {
      get { return _sessionStorage; }
      set { _sessionStorage = value; }
    }

    public static ISession Session
    {
      get { return _sessionStorage.Session; }
      set { _sessionStorage.Session = value; }
    }

    public static bool HasSession
    {
      get { return _sessionStorage.HasSession; }
    }
  }

  public interface ISessionStorage
  {
    ISession Session
    {
      get;
      set;
    }

    bool HasSession { get; }
  }

  public abstract class SessionStorageBase : ISessionStorage
  {
    protected abstract ISession InternalSession
    {
      get;
      set;
    }

    public ISession Session
    {
      get
      {
        if (InternalSession == null)
        {
          throw new NoNHibernateSessionException();
        }
        return InternalSession;
      }
      set
      {
        if (InternalSession != value)
        {
          if (InternalSession != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Session when one is already in use!");
          }
        }
        InternalSession = value;
      }
    }

    public bool HasSession
    {
      get { return InternalSession != null; }
    }
  }

  public class HttpContextSessionStorage : SessionStorageBase
  {
    protected override ISession InternalSession
    {
      get { return (ISession)HttpContext.Current.Items[typeof(HttpContextSessionStorage).FullName]; }
      set { HttpContext.Current.Items[typeof(HttpContextSessionStorage).FullName] = value; }
    }
  }

  public class ThreadStaticSessionStorage : SessionStorageBase
  {
    [ThreadStatic]
    private static ISession _session;

    protected override ISession InternalSession
    {
      get { return _session; }
      set { _session = value; }
    }
  }

  public class ThreadStaticAndHttpContextSessionStorage : ISessionStorage
  {
    public ISession Session
    {
      get { return this.CurrentStorage.Session; }
      set { this.CurrentStorage.Session = value; }
    }

    public bool HasSession
    {
      get { return this.CurrentStorage.HasSession; }
    }

    ISessionStorage CurrentStorage
    {
      get
      {
        if (HttpContext.Current == null)
          return _threadStaticAndHttpContextSessionStorage;
        return _httpContextSessionStorage;
      }
    }

    readonly ISessionStorage _threadStaticAndHttpContextSessionStorage = new ThreadStaticSessionStorage();
    readonly ISessionStorage _httpContextSessionStorage = new HttpContextSessionStorage();
  }
}