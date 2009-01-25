using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class NullUnitOfWorkEvents : IUnitOfWorkEvents
  {
    public void Start(IUnitOfWork unitOfWork)
    {
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
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
    }

    public void Dispose(IUnitOfWork unitOfWork)
    {
    }
  }
}
