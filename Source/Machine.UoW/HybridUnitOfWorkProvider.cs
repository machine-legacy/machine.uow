using System;
using System.Collections.Generic;
using System.Web;

namespace Machine.UoW
{
  public class HybridUnitOfWorkProvider : IUnitOfWorkProvider
  {
    readonly HttpContextUnitOfWorkProvider _httpContextUnitOfWorkProvider;
    readonly ThreadStaticUnitOfWorkProvider _threadStaticUnitOfWorkProvider;

    public HybridUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _httpContextUnitOfWorkProvider = new HttpContextUnitOfWorkProvider(unitOfWorkFactory);
      _threadStaticUnitOfWorkProvider = new ThreadStaticUnitOfWorkProvider(unitOfWorkFactory);
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      return GetUnitOfWorkProviderToUse().Start(settings);
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return GetUnitOfWorkProviderToUse().GetUnitOfWork();
    }

    private IUnitOfWorkProvider GetUnitOfWorkProviderToUse()
    {
      if (HttpContext.Current != null)
      {
        return _httpContextUnitOfWorkProvider;
      }
      return _threadStaticUnitOfWorkProvider;
    }
  }
}
