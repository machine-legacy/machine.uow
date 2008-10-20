using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NHibernateUoWEvents : IUnitOfWorkEvents
  {
    private readonly ISessionFactory _sessionFactory;

    public NHibernateUoWEvents(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    #region IUnitOfWorkEvents Members
    public void Start(IUnitOfWork unitOfWork)
    {
      ISession session = _sessionFactory.OpenSession();
      ITransaction transaction = session.BeginTransaction();
      unitOfWork.Set(new CurrentSession(session, transaction));
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Rollback(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Rollback(IUnitOfWork unitOfWork)
    {
      unitOfWork.Get<CurrentSession>().Rollback();
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
      unitOfWork.Get<CurrentSession>().Commit();
    }
    #endregion
  }
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
      this.Session.Close();
    }

    public void Commit()
    {
      this.Transaction.Commit();
      this.Session.Close();
    }
  }
}
