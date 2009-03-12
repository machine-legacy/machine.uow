using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkFactory
  {
    IUnitOfWork StartUnitOfWork(IUnitOfWorkScope scope);
    IUnitOfWorkScope StartScope(IUnitOfWorkSettings[] allSettings);
  }
}