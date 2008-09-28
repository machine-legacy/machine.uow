using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkManagement : IUnitOfWorkManagement
  {
    private IUnitOfWorkEvents _globalEvents;

    public void AddEvents(IUnitOfWorkEvents unitOfWorkEvents)
    {
      _globalEvents = unitOfWorkEvents;
    }

    public IUnitOfWorkEvents FindEventsFor(object instance)
    {
      return _globalEvents;
    }
  }
}
