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

    #region IUnitOfWorkEvents Members

    public void Start(IUnitOfWork unitOfWork)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Start(unitOfWork);
      }
    }

    public void AddNew(object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.AddNew(obj);
      }
    }

    public void Save(object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Save(obj);
      }
    }

    public void Delete(object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Delete(obj);
      }
    }

    public void Rollback(object obj)
    {
      foreach (IUnitOfWorkEvents events in _events)
      {
        events.Rollback(obj);
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
    #endregion
  }
}
