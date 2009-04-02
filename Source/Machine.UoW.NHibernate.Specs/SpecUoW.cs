using System;
using System.Collections.Generic;
using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public static class SpecUoW
  {
    public static Func<IUnitOfWorkScope> Scope;
    
    public static Func<IUnitOfWorkSettings[], IUnitOfWork> StartUoW;

    public static IUnitOfWork Start(params IUnitOfWorkSettings[] settings)
    {
      return StartUoW(settings);
    }

    public static Func<IManagedSession> OpenSession;

    public static Func<IManagedConnection> OpenConnection;

    public static void Startup(IUnitOfWorkProvider provider, IUnitOfWorkScopeProvider scopeProvider, ISessionManager sessionManager, IConnectionManager connectionManager)
    {
      Scope = () => scopeProvider.GetUnitOfWorkScope();
      StartUoW = (settings) => provider.Start(Scope(), settings);
      OpenSession = () => sessionManager.OpenSession(String.Empty);
      OpenConnection = () => connectionManager.OpenConnection(String.Empty);
    }
  }
}