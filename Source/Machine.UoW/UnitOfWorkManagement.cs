using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkManagement : IUnitOfWorkManagement
  {
    private readonly List<IUnitOfWorkEvents> _events = new List<IUnitOfWorkEvents>();

    public void AddEvents(IUnitOfWorkEvents unitOfWorkEvents)
    {
      _events.Add(unitOfWorkEvents);
    }

    public IUnitOfWorkEvents FindEventsFor(object instance)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        if (events.AppliesToObject(instance))
        {
          return events;
        }
      }
      throw new InvalidOperationException("No application Events implementation found for: " + instance);
    }
  }
}
