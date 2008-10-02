using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkEvents
  {
    void Start(IUnitOfWork unitOfWork);
    void AddNew(object obj);
    void Save(object obj);
    void Delete(object obj);
    void Rollback(object obj);
    void Rollback(IUnitOfWork unitOfWork);
    void Commit(IUnitOfWork unitOfWork);
  }
}