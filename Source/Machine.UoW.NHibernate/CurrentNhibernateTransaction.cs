using System;
using System.Collections.Generic;

using INHibernateTransaction = NHibernate.ITransaction;

namespace Machine.UoW.NHibernate
{
  public class CurrentNhibernateTransaction : ITransaction
  {
    readonly INHibernateTransaction _transaction;

    public CurrentNhibernateTransaction(INHibernateTransaction transaction)
    {
      _transaction = transaction;
    }
    
    public void Begin()
    {
    }

    public void Commit()
    {
      _transaction.Commit();
    }

    public void Rollback()
    {
      _transaction.Rollback();
    }

    public void Dispose()
    {
      _transaction.Dispose();
    }
  }
}
