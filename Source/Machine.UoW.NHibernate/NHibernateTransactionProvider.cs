using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.NHibernate
{
  public class NHibernateTransactionProvider : ITransactionProvider
  {
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public NHibernateTransactionProvider(IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      return new CurrentNhibernateTransaction(scope.Session().BeginTransaction(isolationLevel));
    }
  }
}
