using System;

namespace Machine.UoW
{
  public class ThreadStaticUnitOfWorkProvider : IUnitOfWorkProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;

    [ThreadStatic]
    static IUnitOfWork _unitOfWork;

    public ThreadStaticUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      _unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkFactory.StartScope(settings));
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