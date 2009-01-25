using System;
using System.Data;

using NHibernate;
using NHibernate.Transaction;

namespace Machine.UoW.NHibernate
{
  public class NullTransaction : ITransaction
  {
    public void Dispose()
    {
    }

    public void Begin()
    {
    }

    public void Begin(IsolationLevel isolationLevel)
    {
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public void Enlist(IDbCommand command)
    {
    }

    public void RegisterSynchronization(ISynchronization synchronization)
    {
    }

    public bool IsActive
    {
      get { return false; }
    }

    public bool WasRolledBack
    {
      get { return false; }
    }

    public bool WasCommitted
    {
      get { return false; }
    }
  }
}