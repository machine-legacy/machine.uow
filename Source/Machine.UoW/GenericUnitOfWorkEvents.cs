using System;

namespace Machine.UoW
{
  public class GenericUnitOfWorkEvents<T> : IUnitOfWorkEvents
  {
    public virtual void Start(IUnitOfWork unitOfWork)
    {
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
      AddNew(unitOfWork, (T)obj);
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
      Save(unitOfWork, (T)obj);
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
      Delete(unitOfWork, (T)obj);
    }

    public void Rollback(IUnitOfWork unitOfWork, object obj)
    {
      Rollback(unitOfWork, (T)obj);
    }

    public virtual void Rollback(IUnitOfWork unitOfWork)
    {
    }

    public virtual void Commit(IUnitOfWork unitOfWork)
    {
    }

    public virtual void AddNew(IUnitOfWork unitOfWork, T obj)
    {
    }

    public virtual void Save(IUnitOfWork unitOfWork, T obj)
    {
    }

    public virtual void Delete(IUnitOfWork unitOfWork, T obj)
    {
    }

    public virtual void Rollback(IUnitOfWork unitOfWork, T obj)
    {
    }

    public virtual void Dispose(IUnitOfWork unitOfWork)
    {
    }
  }
}