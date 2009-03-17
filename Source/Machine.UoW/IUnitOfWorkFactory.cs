using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkFactory
  {
    IUnitOfWork StartUnitOfWork(IUnitOfWorkScope scope);
    IUnitOfWorkScope StartScope(IUnitOfWorkScope parentScope, IUnitOfWorkSettings[] allSettings);
    IUnitOfWorkScope StartScope(IUnitOfWorkSettings[] allSettings);
  }
}