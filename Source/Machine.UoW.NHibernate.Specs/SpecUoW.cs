using System;
using System.Collections.Generic;

using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public static class SpecUoW
  {
    public static Func<IUnitOfWork> StartUoW;

    public static IUnitOfWork Start()
    {
      return StartUoW();
    }

    public static Func<IManagedSession> OpenSession;

    public static Func<IManagedConnection> OpenConnection;

    public static void Startup(IUnitOfWorkProvider provider, IUnitOfWorkScopeProvider scopeProvider, ISessionManager sessionManager, IConnectionManager connectionManager)
    {
      StartUoW = () => provider.Start(scopeProvider.GetUnitOfWorkScope(), new IUnitOfWorkSettings[0]);
      OpenSession = () => sessionManager.OpenSession(String.Empty);
      OpenConnection = () => connectionManager.OpenConnection(String.Empty);
    }
  }
}
