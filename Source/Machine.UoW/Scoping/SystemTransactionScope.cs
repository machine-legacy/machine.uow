using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW.Scoping
{
  public class SystemTransactionScope : ITransactionScope
  {
    readonly System.Transactions.TransactionScope _scope;

    public SystemTransactionScope(TransactionScopeOption scopeOption)
    {
      _scope = new System.Transactions.TransactionScope(scopeOption);
    }

    public void Complete()
    {
      _scope.Complete();
    }

    public void Dispose()
    {
      _scope.Dispose();
    }
  }

  public class NullTransactionScope : ITransactionScope
  {
    public void Dispose()
    {
    }

    public void Complete()
    {
    }
  }

  public interface ITransactionScope : IDisposable
  {
    void Complete();
  }
}
