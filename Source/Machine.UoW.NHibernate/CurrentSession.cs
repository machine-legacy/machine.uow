using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class CurrentSession
  {
    readonly ISession _session;
    readonly ITransaction _transaction;

    public ISession Session
    {
      get { return _session; }
    }

    public CurrentSession(ISession session)
    {
      _session = session;
      _transaction = new NullTransaction();
    }

    public CurrentSession(ISession session, ITransaction transaction)
    {
      _session = session;
      _transaction = transaction;
    }

    public void Rollback()
    {
      _session.Clear();
      _transaction.Rollback();
    }

    public void Commit()
    {
      _session.Flush();
      _transaction.Commit();
    }

    public void Dispose()
    {
    }
  }
}