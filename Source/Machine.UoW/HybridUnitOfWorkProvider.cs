using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web;

using Machine.UoW.AmbientTransactions;

namespace Machine.UoW
{
  public class HybridUnitOfWorkProvider : IUnitOfWorkProvider
  {
    readonly HttpContextUnitOfWorkProvider _httpContextUnitOfWorkProvider;
    readonly ThreadStaticUnitOfWorkProvider _threadStaticUnitOfWorkProvider;
    readonly AmbientTransactionUoWProvider _ambientTransactionUoWProvider;

    public HybridUnitOfWorkProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _httpContextUnitOfWorkProvider = new HttpContextUnitOfWorkProvider(unitOfWorkFactory, new HttpContextUnitOfWorkScopeProvider(unitOfWorkFactory));
      _threadStaticUnitOfWorkProvider = new ThreadStaticUnitOfWorkProvider(unitOfWorkFactory, new ThreadStaticUnitOfWorkScopeProvider(unitOfWorkFactory));
      _ambientTransactionUoWProvider = new AmbientTransactionUoWProvider(unitOfWorkFactory, new AmbientTransactionUnitOfWorkScopeProvider(unitOfWorkFactory));
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
      if (Transaction.Current != null)
      {
        return _ambientTransactionUoWProvider;
      }
      if (HttpContext.Current != null)
      {
        return _httpContextUnitOfWorkProvider;
      }
      return _threadStaticUnitOfWorkProvider;
    }
  }
}
