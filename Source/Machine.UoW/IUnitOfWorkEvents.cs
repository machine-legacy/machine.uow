using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkEvents
  {
    void Start(IUnitOfWork unitOfWork);
    void AddNew(IUnitOfWork unitOfWork, object obj);
    void Save(IUnitOfWork unitOfWork, object obj);
    void Delete(IUnitOfWork unitOfWork, object obj);
    void Rollback(IUnitOfWork unitOfWork, object obj);
    void Rollback(IUnitOfWork unitOfWork);
    void Commit(IUnitOfWork unitOfWork);
  }
}