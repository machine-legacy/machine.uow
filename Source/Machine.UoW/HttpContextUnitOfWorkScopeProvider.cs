using System;
using System.Web;

namespace Machine.UoW
{
  public class HttpContextUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    static readonly string Key = typeof (HttpContextUnitOfWorkScopeProvider).FullName;
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScope _globalScope;

    public HttpContextUnitOfWorkScopeProvider(IUnitOfWorkScope globalScope, IUnitOfWorkFactory unitOfWorkFactory)
    {
      _globalScope = globalScope;
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope()
    {
      if (CurrentScope == null)
      {
        CurrentScope = _unitOfWorkFactory.StartScope(_globalScope, new IUnitOfWorkSettings[0]);
        CurrentScope.Disposed += OnUnitOfWorkScopeDisposed;
      }
      return CurrentScope;
    }

    private static void OnUnitOfWorkScopeDisposed(object sender, EventArgs e)
    {
      CurrentScope = null;
    }

    private static IUnitOfWorkScope CurrentScope
    {
      get { return (IUnitOfWorkScope)HttpContext.Current.Items[Key]; }
      set { HttpContext.Current.Items[Key] = value; }
    }
  }
}