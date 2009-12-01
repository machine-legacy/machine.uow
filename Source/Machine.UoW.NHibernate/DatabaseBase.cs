using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class DatabaseAndSessionBase
  {
    static ISessionManager _sessionManager;
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DatabaseAndSessionBase));

    public static IManagedSession OpenSession()
    {
      return _sessionManager.OpenSession();
    }

    public static ISession Session
    {
      get { return _sessionManager.CurrentSession(); }
    }

    public static void Startup(ISessionManager manager)
    {
      _sessionManager = manager;
    }

    public static void StartupNull()
    {
      Startup(new NullSessionManager());
    }

    public static void With(Action action)
    {
      using (var session = OpenSession())
      {
        action();
        session.Commit();
      }
    }

    public static T With<T>(Func<T> func)
    {
      using (var session = OpenSession())
      {
        var value = func();
        session.Commit();
        return value;
      }
    }
  }
}
