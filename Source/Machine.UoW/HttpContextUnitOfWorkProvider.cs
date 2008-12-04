using System;
using System.Web;

namespace Machine.UoW
{
  public class HttpContextUnitOfWorkProvider : IUnitOfWorkProvider
  {
    private static readonly string Key = typeof (HttpContextUnitOfWorkProvider).FullName;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public HttpContextUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    #region IUnitOfWorkProvider Members
    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      CurrentUnitOfWork state = State;
      state.UnitOfWork = _unitOfWorkFactory.StartUnitOfWork(settings);
      return state.UnitOfWork;
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return State.UnitOfWork;
    }
    #endregion

    private static HttpContext Current
    {
      get { return HttpContext.Current; }
    }

    private static CurrentUnitOfWork State
    {
      get
      {
        if (Current.Items[Key] == null)
        {
          Current.Items[Key] = new CurrentUnitOfWork();
        }
        return (CurrentUnitOfWork)Current.Items[Key];
      }
    }

    public class CurrentUnitOfWork
    {
      public IUnitOfWork UnitOfWork;
    }
  }
}