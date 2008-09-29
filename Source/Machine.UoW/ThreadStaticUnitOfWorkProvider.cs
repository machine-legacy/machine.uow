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

    #region IUnitOfWorkProvider Members
    public IUnitOfWork Start()
    {
      _unitOfWork = _unitOfWorkFactory.StartUnitOfWork();
      return GetUnitOfWork();
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return _unitOfWork;
    }
    #endregion
  }
}