using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class ThreadStaticUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    [ThreadStatic]
    static IUnitOfWorkScope _unitOfWorkScope;

    public ThreadStaticUnitOfWorkScopeProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope(IUnitOfWorkSettings[] settings)
    {
      if (_unitOfWorkScope == null)
      {
        _unitOfWorkScope = _unitOfWorkFactory.StartScope(settings);
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