using System;
using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Machine.UoW.NHibernate
{
  [Serializable]
  public class WebMultipleSessionContext : MapBasedSessionContext
  {
    private readonly string SessionFactoryMapKey = "WebSessionContext.SessionFactoryMapKey.";

    public WebMultipleSessionContext(ISessionFactoryImplementor factory)
      : base(factory)
    {
      SessionFactoryMapKey += Guid.NewGuid().ToString();
    }

    protected override IDictionary GetMap()
    {
      return (HttpContext.Current.Items[SessionFactoryMapKey] as IDictionary);
    }

    protected override void SetMap(IDictionary value)
    {
      HttpContext.Current.Items[SessionFactoryMapKey] = value;
    }
  }

  [Serializable]
  public class ThreadMultipleSessionContext : MapBasedSessionContext
  {
    [ThreadStatic]
    private static IDictionary _dictionary;

    public ThreadMultipleSessionContext(ISessionFactoryImplementor factory)
      : base(factory)
    {
    }

    protected override IDictionary GetMap()
    {
      return _dictionary;
    }

    protected override void SetMap(IDictionary value)
    {
      _dictionary = value;
    }
  }

  [Serializable]
  public class ThreadOrWebMultipleSessionContext : MapBasedSessionContext
  {
    readonly string SessionFactoryMapKey = "WebSessionContext.SessionFactoryMapKey.";
    [ThreadStatic]
    static IDictionary _dictionary;

    public ThreadOrWebMultipleSessionContext(ISessionFactoryImplementor factory)
      : base(factory)
    {
      SessionFactoryMapKey += Guid.NewGuid().ToString();
    }

    protected override IDictionary GetMap()
    {
      if (HttpContext.Current != null)
        return (HttpContext.Current.Items[SessionFactoryMapKey] as IDictionary);
      return _dictionary;
    }

    protected override void SetMap(IDictionary value)
    {
      if (HttpContext.Current != null)
      {
        HttpContext.Current.Items[SessionFactoryMapKey] = value;
      }
      else
      {
        _dictionary = value;
      }
    }
  }
}