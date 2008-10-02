using System;

namespace Machine.UoW
{
  public class GenericUnitOfWorkEvents<T> : IUnitOfWorkEvents
  {
    #region IUnitOfWorkEvents Members
    public void AddNew(object obj)
    {
      AddNew((T)obj);
    }

    public void Save(object obj)
    {
      Save((T)obj);
    }

    public void Delete(object obj)
    {
      Delete((T)obj);
    }

    public void Rollback(object obj)
    {
      Rollback((T)obj);
    }
    #endregion

    public virtual void AddNew(T obj)
    {
    }

    public virtual void Save(T obj)
    {
    }

    public virtual void Delete(T obj)
    {
    }

    public virtual void Rollback(T obj)
    {
    }
  }
}