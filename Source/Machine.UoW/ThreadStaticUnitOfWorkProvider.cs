using System;

namespace Machine.UoW
{
  public class ThreadStaticUnitOfWorkProvider : IUnitOfWorkProvider
  {
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    [ThreadStatic]
    private static IUnitOfWork _unitOfWork;

    public ThreadStaticUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      _unitOfWork = _unitOfWorkFactory.StartUnitOfWork(settings);
      _unitOfWork.Closed += OnClosed;
      return GetUnitOfWork();
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return _unitOfWork;
    }

    private static void OnClosed(object sender, EventArgs e)
    {
      _unitOfWork = null;
    }
  }
}