using System;
using System.Web;

namespace Machine.UoW
{
  public class HttpContextUnitOfWorkProvider : IUnitOfWorkProvider
  {
    static readonly string Key = typeof (HttpContextUnitOfWorkProvider).FullName;
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public HttpContextUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      IUnitOfWork unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope(settings));
      unitOfWork.Closed += OnClosed;
      CurrentUoW = unitOfWork;
      return CurrentUoW;
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return CurrentUoW;
    }

    private static void OnClosed(object sender, EventArgs e)
    {
      CurrentUoW = null;
    }

    private static IUnitOfWork CurrentUoW
    {
      get { return (IUnitOfWork)HttpContext.Current.Items[Key]; }
      set { HttpContext.Current.Items[Key] = value; }
    }
  }

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