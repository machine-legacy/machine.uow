using System;
using System.Web;

namespace Machine.UoW
{
  public class HttpContextUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    static readonly string Key = typeof (HttpContextUnitOfWorkScopeProvider).FullName;
    readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public HttpContextUnitOfWorkScopeProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings)
    {
      if (CurrentScope == null)
      {
        CurrentScope = _unitOfWorkFactory.StartScope(settings);
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