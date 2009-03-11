using System;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class CurrentSession : IDisposable
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CurrentSession));
    readonly ISession _session;
    readonly ITransaction _transaction;

    public ISession Session
    {
      get { return _session; }
    }

    public CurrentSession(ISession session, ITransaction transaction)
    {
      _session = session;
      _transaction = transaction;
    }

    public void Rollback()
    {
      _log.Info("Rollback");
      _session.Clear();
      _transaction.Rollback();
    }

    public void Commit()
    {
      _log.Info("Commit");
      _session.Flush();
      _transaction.Commit();
    }

    public void Dispose()
    {
      _log.Info("Dispose");
      _session.Dispose();
    }
  }
}