using System;
using System.Collections.Generic;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkScopeFactory _unitOfWorkScopeFactory;
    readonly IUnitOfWorkScope _globalScope;

    public AmbientTransactionUnitOfWorkScopeProvider(IUnitOfWorkScope globalScope, IUnitOfWorkScopeFactory unitOfWorkScopeFactory)
    {
      _globalScope = globalScope;
      _unitOfWorkScopeFactory = unitOfWorkScopeFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope()
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      IUnitOfWorkScope scope = state.Get<IUnitOfWorkScope>();
      if (scope == null)
      {
        scope = _unitOfWorkScopeFactory.StartScope(_globalScope, new IUnitOfWorkSettings[0]);
        scope.Disposed += OnUnitOfWorkScopeDisposed;
        state.Set<IUnitOfWorkScope>(scope);
      }
      return scope;
    }

    private static void OnUnitOfWorkScopeDisposed(object sender, EventArgs e)
    {
      TransactionState.ClearFromEveryScope((IUnitOfWorkScope)sender);
    }
  }
}