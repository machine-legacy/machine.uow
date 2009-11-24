using System;
using System.Collections.Generic;

using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public static class SpecDatabase
  {
    public static Func<IManagedSession> OpenSession;

    public static Func<IManagedConnection> OpenConnection;

    public static void Startup(ISessionManager sessionManager, IConnectionManager connectionManager)
    {
      OpenSession = () => sessionManager.OpenSession();
      OpenConnection = () => connectionManager.OpenConnection(String.Empty);
    }
  }
}
