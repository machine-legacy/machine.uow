using System;
using System.Collections.Generic;

using Machine.UoW.DatabaseContext;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class NH
  {
    static IContextStorage<ISession> _sessionStorage = new ThreadStaticStorage<ISession>();

    public static IContextStorage<ISession> Storage
    {
      get { return _sessionStorage; }
      set { _sessionStorage = value; }
    }

    public static ISession Session
    {
      get { return _sessionStorage.Peek(); }
    }

    public static bool HasSession
    {
      get { return !_sessionStorage.IsEmpty; }
    }
  }
}