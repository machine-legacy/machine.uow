using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkManagement : IUnitOfWorkManagement
  {
    public void AddGlobalEvents(IUnitOfWorkEvents unitOfWorkEvents)
    {
      throw new System.NotImplementedException();
    }

    public void AddEventsFor<T>(IUnitOfWorkEvents unitOfWorkEvents)
    {
      throw new System.NotImplementedException();
    }

    public IUnitOfWorkEvents FindEventsFor(Type objectType)
    {
      throw new System.NotImplementedException();
    }
  }
}
