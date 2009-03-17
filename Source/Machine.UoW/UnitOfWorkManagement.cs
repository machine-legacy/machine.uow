using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkManagement : IUnitOfWorkManagement
  {
    readonly List<IUnitOfWorkEvents> _events = new List<IUnitOfWorkEvents>();
    readonly List<IScopeEvents> _scopeEvents = new List<IScopeEvents>();

    public void AddEvents(IUnitOfWorkEvents unitOfWorkEvents)
    {
      _events.Add(unitOfWorkEvents);
    }

    public void AddEvents(IScopeEvents scopeEvents)
    {
      _scopeEvents.Add(scopeEvents);
    }

    public UnitOfWorkEventsProxy GetUnitOfWorkEventsProxy()
    {
      return new UnitOfWorkEventsProxy(_events);
    }

    public ScopeEventsProxy GetScopeEventsProxy()
    {
      return new ScopeEventsProxy(_scopeEvents);
    }
  }

  public class ScopeEventsProxy  : IScopeEvents
  {
    readonly List<IScopeEvents> _events;

    public ScopeEventsProxy(List<IScopeEvents> events)
    {
      _events = events;
    }

    public void Start(IUnitOfWorkScope scope)
    {
      foreach (IScopeEvents events in _events)
      {
        events.Start(scope);
      }
    }
  }

  public class UnitOfWorkEventsProxy : IUnitOfWorkEvents
  {
    readonly List<IUnitOfWorkEvents> _events;

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
