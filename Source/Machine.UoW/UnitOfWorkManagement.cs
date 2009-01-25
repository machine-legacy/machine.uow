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

    public UnitOfWorkEventsProxy GetUnitOfWorkEventsProxy()
    {
      if (_events.Count == 0)
      {
        throw new InvalidOperationException("No UnitOfWorkEvents have been registered! Nothing is happening!");
      }
      return new UnitOfWorkEventsProxy(_events);
    }
  }
  public class UnitOfWorkEventsProxy : IUnitOfWorkEvents
  {
    private readonly List<IUnitOfWorkEvents> _events;

    public UnitOfWorkEventsProxy(List<IUnitOfWorkEvents> events)
    {
      _events = events;
    }

    public void Start(IUnitOfWork unitOfWork)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Start(unitOfWork);
      }
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.AddNew(unitOfWork, obj);
      }
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Save(unitOfWork, obj);
      }
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Delete(unitOfWork, obj);
      }
    }

    public void Rollback(IUnitOfWork unitOfWork, object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Rollback(unitOfWork, obj);
      }
    }

    public void Rollback(IUnitOfWork unitOfWork)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Rollback(unitOfWork);
      }
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Commit(unitOfWork);
      }
    }

    public void Dispose(IUnitOfWork unitOfWork)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Dispose(unitOfWork);
      }
    }
  }
}
