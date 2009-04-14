using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW.Scoping
{
  public class TransactionScope
  {
    public static Func<TransactionScopeOption, ITransactionScope> OpenScope = (option) => new SystemTransactionScope(option);

    public static void UseNull()
    {
      OpenScope = (option) => new NullTransactionScope();
    }

    public static void UseSystemTransaction()
    {
      OpenScope = (option) => new SystemTransactionScope(option);
    }

    public static ITransactionScope Open(TransactionScopeOption option)
    {
      return OpenScope(option);
    }

    public static ITransactionScope OpenReadOnly()
    {
      return Open(TransactionScopeOption.Required);
    }

    public static ITransactionScope Open()
    {
      return Open(TransactionScopeOption.Required);
    }
  }
}
