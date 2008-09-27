using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkManagement : IUnitOfWorkManagement
  {
    private IUnitOfWorkEvents _globalEvents;

    public void AddGlobalEvents(IUnitOfWorkEvents unitOfWorkEvents)
    {
      _globalEvents = unitOfWorkEvents;
    }

    public void AddEventsFor<T>(IUnitOfWorkEvents unitOfWorkEvents)
    {
      throw new System.NotImplementedException();
    }

    public IUnitOfWorkEvents FindEventsFor(Type objectType)
    {
      return _globalEvents;
    }
  }
}
