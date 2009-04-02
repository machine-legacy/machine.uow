using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkFactory : IUnitOfWorkScopeFactory
  {
    IUnitOfWork StartUnitOfWork(IUnitOfWorkScope scope);
  }
  
  public interface IUnitOfWorkScopeFactory
  {
    IUnitOfWorkScope StartScope(IUnitOfWorkScope parentScope, IUnitOfWorkSettings[] allSettings);
  }
}