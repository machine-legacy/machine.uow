using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkManagement
  {
    void AddGlobalEvents(IUnitOfWorkEvents unitOfWorkEvents);
    void AddEventsFor<T>(IUnitOfWorkEvents unitOfWorkEvents);
    IUnitOfWorkEvents FindEventsFor(Type objectType);
  }
}