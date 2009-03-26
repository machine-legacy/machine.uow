using System;
using System.Collections.Generic;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScope _globalScope;

    public AmbientTransactionUnitOfWorkScopeProvider(IUnitOfWorkScope globalScope, IUnitOfWorkFactory unitOfWorkFactory)
    {
      _globalScope = globalScope;
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings)
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      IUnitOfWorkScope scope = state.Get<IUnitOfWorkScope>();
      if (scope == null)
      {
        scope = _unitOfWorkFactory.StartScope(_globalScope, settings);
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