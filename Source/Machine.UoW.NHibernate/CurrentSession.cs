using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class CurrentSession
  {
    public ISession Session { get; private set; }
    public ITransaction Transaction { get; private set; }

    public CurrentSession(ISession session, ITransaction transaction)
    {
      this.Session = session;
      this.Transaction = transaction;
    }

    public void Rollback()
    {
      this.Transaction.Rollback();
    }

    public void Commit()
    {
      this.Transaction.Commit();
    }
  }
}