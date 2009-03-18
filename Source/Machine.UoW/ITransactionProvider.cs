using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW
{
  public interface ITransactionProvider
  {
    ITransaction BeginTransaction(IsolationLevel isolationLevel);
  }

  public class NullTransactionProvider : ITransactionProvider
  {
    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      return new NullTransaction();
    }
  }
}
