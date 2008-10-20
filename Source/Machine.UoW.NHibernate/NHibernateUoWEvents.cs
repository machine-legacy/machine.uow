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
      unitOfWork.Set(session);
      unitOfWork.Set(transaction);
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
      unitOfWork.Get<ITransaction>().Rollback();
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
      unitOfWork.Get<ITransaction>().Commit();
    }
    #endregion
  }
}
