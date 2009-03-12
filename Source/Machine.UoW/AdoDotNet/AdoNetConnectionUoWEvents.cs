using System;
using System.Collections.Generic;

namespace Machine.UoW.AdoDotNet
{
  public class AdoNetConnectionScopeEvents : IScopeEvents, IScopeProvider
  {
    readonly IConnectionProvider _connectionProvider;

    public AdoNetConnectionScopeEvents(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    public void Start(IUnitOfWorkScope scope)
    {
      scope.Add(typeof(CurrentConnection), this);
    }

    public IDisposable Create()
    {
      return new CurrentConnection(_connectionProvider);
    }
  }
}
