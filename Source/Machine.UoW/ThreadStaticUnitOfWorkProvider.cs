using System;

namespace Machine.UoW
{
  public class ThreadStaticUnitOfWorkProvider : IUnitOfWorkProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    [ThreadStatic]
    static IUnitOfWork _unitOfWork;

    public ThreadStaticUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      _unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope(settings));
      _unitOfWork.Closed += OnUnitOfWorkClosed;
      return GetUnitOfWork();
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return _unitOfWork;
    }

    private static void OnUnitOfWorkClosed(object sender, EventArgs e)
    {
      _unitOfWork = null;
    }
  }

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