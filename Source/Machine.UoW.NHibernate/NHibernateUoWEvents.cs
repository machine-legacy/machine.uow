using System;
using System.Collections.Generic;

namespace Machine.UoW.NHibernate
{
  public class NHibernateUoWEvents : IUnitOfWorkEvents
  {
    public void Start(IUnitOfWork unitOfWork)
    {
      unitOfWork.Scope.Get<CurrentNhibernateTransaction>().Begin();
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Scope.Session().Save(obj);
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Scope.Session().Save(obj);
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
      unitOfWork.Scope.Session().Delete(obj);
    }

    public void Rollback(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Rollback(IUnitOfWork unitOfWork)
    {
      unitOfWork.Scope.Get<CurrentNhibernateTransaction>().Rollback();
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
      unitOfWork.Scope.Get<CurrentNhibernateTransaction>().Commit();
    }

    public void Dispose(IUnitOfWork unitOfWork)
    {
    }
  }
}
