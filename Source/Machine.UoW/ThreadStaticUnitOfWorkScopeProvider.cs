using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class ThreadStaticUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScope _globalScope;
    [ThreadStatic]
    static IUnitOfWorkScope _unitOfWorkScope;

    public ThreadStaticUnitOfWorkScopeProvider(IUnitOfWorkScope globalScope, IUnitOfWorkFactory unitOfWorkFactory)
    {
      _globalScope = globalScope;
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope(IUnitOfWorkSettings[] settings)
    {
      if (_unitOfWorkScope == null)
      {
        _unitOfWorkScope = _unitOfWorkFactory.StartScope(_globalScope, settings);
        _unitOfWorkScope.Disposed += OnUnitOfWorkScopeDisposed;
      }
      return _unitOfWorkScope;
    }

    private static void OnUnitOfWorkScopeDisposed(object sender, EventArgs e)
    {
      _unitOfWorkScope = null;
    }
  }
}