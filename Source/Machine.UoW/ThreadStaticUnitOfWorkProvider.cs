using System;
using System.Collections.Generic;

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

    public IUnitOfWork Start(IUnitOfWorkScope scope, IUnitOfWorkSettings[] settings)
    {
      _unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkFactory.StartScope(scope, settings));
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
}