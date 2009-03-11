using System;
using System.Collections.Generic;

using Machine.UoW.AdoDotNet;

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

    public void Start(IUnitOfWork unitOfWork)
    {
      NHibernateSessionSettings settings = unitOfWork.Get(NHibernateSessionSettings.Default);
      ISession session = _sessionFactory.OpenSession(unitOfWork.Connection());
      session.FlushMode = settings.FlushMode;
      ITransaction transaction = session.BeginTransaction(settings.IsolationLevel);
      unitOfWork.Set(new CurrentSession(session, transaction));
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Get<CurrentSession>().Session.Save(obj);
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Get<CurrentSession>().Session.Save(obj);
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Get<CurrentSession>().Session.Delete(obj);
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

    public void Dispose(IUnitOfWork unitOfWork)
    {
      unitOfWork.Get<CurrentSession>().Dispose();
    }
  }
}
