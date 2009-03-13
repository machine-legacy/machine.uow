using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public static class UoW
  {
    static IUnitOfWorkProvider _provider;
    static IUnitOfWorkScopeProvider _scopeProvider;

    public static IUnitOfWorkProvider Provider
    {
      get { return _provider; }
      set { _provider = value; }
    }

    public static IUnitOfWorkScopeProvider ScopeProvider
    {
      get { return _scopeProvider; }
      set { _scopeProvider = value; }
    }

    public static IUnitOfWorkScope Scope
    {
      get { return _scopeProvider.GetUnitOfWorkScope(); }
    }

    public static IUnitOfWork Current
    {
      get { return _provider.GetUnitOfWork(); }
    }

    public static IUnitOfWork Start(params IUnitOfWorkSettings[] settings)
    {
      return _provider.Start(Scope, settings);
    }
  }
}
