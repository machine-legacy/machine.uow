using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkManagement
  {
    void AddEvents(IUnitOfWorkEvents unitOfWorkEvents);
    IUnitOfWorkEvents FindEventsFor(Type objectType);
  }
}