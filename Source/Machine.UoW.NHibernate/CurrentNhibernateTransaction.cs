using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class CurrentNhibernateTransaction : IDisposable
  {
    readonly ITransaction _transaction;

    public CurrentNhibernateTransaction(ITransaction transaction)
    {
      _transaction = transaction;
    }
    
    public void Begin()
    {
    }

    public void Commit(IUnitOfWorkScope scope)
    {
      System.Diagnostics.Debug.WriteLine("COMMIT");
      scope.Session().Flush();
      _transaction.Commit();
    }

    public void Rollback(IUnitOfWorkScope scope)
    {
      System.Diagnostics.Debug.WriteLine("ROLLBACK");
      _transaction.Rollback();
    }

    public void Dispose()
    {
      _transaction.Dispose();
    }
  }
}
