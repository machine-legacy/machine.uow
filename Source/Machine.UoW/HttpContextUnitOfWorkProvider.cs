using System;
using System.Web;

namespace Machine.UoW
{
  public class HttpContextUnitOfWorkProvider : IUnitOfWorkProvider
  {
    static readonly string Key = typeof (HttpContextUnitOfWorkProvider).FullName;
    readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public HttpContextUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWork Start(IUnitOfWorkScope scope, IUnitOfWorkSettings[] settings)
    {
      IUnitOfWork unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkFactory.StartScope(scope, settings));
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
}