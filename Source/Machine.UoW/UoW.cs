using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW
{
  public static class UoW
  {
    public static Func<IUnitOfWorkScope> Scope;
    
    public static Func<IUnitOfWork> Current;
    
    public static Func<IUnitOfWorkSettings[], IUnitOfWork> StartUoW;

    public static Func<IsolationLevel, ITransaction> BeginTransaction;

    public static IUnitOfWork Start(params IUnitOfWorkSettings[] settings)
    {
      return StartUoW(settings);
    }

    public static void Startup(IUnitOfWorkProvider provider, IUnitOfWorkScopeProvider scopeProvider, ITransactionProvider transactionProvider)
    {
      Scope = () => scopeProvider.GetUnitOfWorkScope();
      Current = () => provider.GetUnitOfWork();
      StartUoW = (settings) => provider.Start(Scope(), settings);
      BeginTransaction = (isolationLevel) => transactionProvider.BeginTransaction(isolationLevel);
    }
  }

  public static class NewUoW
  {
    private static ITransactionProvider _transactionProvider;

    public static ITransaction BeginTransaction()
    {
      return BeginTransaction(IsolationLevel.Unspecified);
    }

    public static ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      return _transactionProvider.BeginTransaction(isolationLevel);
    }

    public static IUnitOfWorkScope Scope()
    {
      return null;
    }

    public static void Startup(ITransactionProvider transactionProvider)
    {
      _transactionProvider = transactionProvider;
    }
  }
}
