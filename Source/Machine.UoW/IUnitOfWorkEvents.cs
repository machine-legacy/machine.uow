using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkEvents
  {
    void AddNew(object obj);
    void Save(object obj);
    void Delete(object obj);
    void Rollback(object obj);
  }
}