using System;
using System.Collections.Generic;

using Machine.UoW.DatabaseContext;
using Machine.UoW.DatabaseContext.Web;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class NH
  {
    static IContextStorage<ISession> _sessionStorage = new ThreadStaticAndHttpContextStorage<ISession>();

    public static IContextStorage<ISession> Storage
    {
      get { return _sessionStorage; }
      set { _sessionStorage = value; }
    }

    public static ISession Session
    {
      get { return _sessionStorage.StoredValue; }
      set { _sessionStorage.StoredValue = value; }
    }

    public static bool HasSession
    {
      get { return _sessionStorage.HasValue; }
    }
  }
}