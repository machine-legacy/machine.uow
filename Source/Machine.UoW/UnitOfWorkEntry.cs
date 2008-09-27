using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkEntry
  {
    private readonly object _instance;
    private readonly List<UnitOfWorkChangeType> _changes;

    public object Instance
    {
      get { return _instance; }
    }

    public IEnumerable<UnitOfWorkChangeType> Changes
    {
      get { return _changes; }
    }

    public UnitOfWorkEntry(object instance)
    {
      _instance = instance;
      _changes = new List<UnitOfWorkChangeType>();
    }

    public void Add()
    {
      if (_changes.Contains(UnitOfWorkChangeType.Saved) || _changes.Contains(UnitOfWorkChangeType.Deleted))
      {
        throw new InvalidOperationException("Adding a object to a UoW when it's been Saved or Deleted already?");
      }
      if (!_changes.Contains(UnitOfWorkChangeType.Added))
      {
        _changes.Add(UnitOfWorkChangeType.Added);
      }
    }

    public void Save()
    {
      if (_changes.Contains(UnitOfWorkChangeType.Deleted))
      {
        throw new InvalidOperationException("Saving an object in a UoW when it's been Deleted already?");
      }
      if (!_changes.Contains(UnitOfWorkChangeType.Saved))
      {
        _changes.Add(UnitOfWorkChangeType.Saved);
      }
    }

    public void Delete()
    {
      if (!_changes.Contains(UnitOfWorkChangeType.Deleted))
      {
        _changes.Add(UnitOfWorkChangeType.Deleted);
      }
    }

    public void Rollback(IUnitOfWorkManagement management)
    {
      IUnitOfWorkEvents events = management.FindEventsFor(_instance.GetType());
      events.Rollback(_instance);
    }

    public void Commit(IUnitOfWorkManagement management)
    {
      IUnitOfWorkEvents events = management.FindEventsFor(_instance.GetType());
      foreach (UnitOfWorkChangeType change in this.ChangesToBeCommitted)
      {
        switch (change)
        {
          case UnitOfWorkChangeType.Added:
            events.AddNew(_instance);
            break;
          case UnitOfWorkChangeType.Saved:
            events.Save(_instance);
            break;
          case UnitOfWorkChangeType.Deleted:
            events.Delete(_instance);
            break;
        }
      }
    }

    private IEnumerable<UnitOfWorkChangeType> ChangesToBeCommitted
    {
      get
      {
        foreach (UnitOfWorkChangeType change in _changes)
        {
          yield return change;
        }
      }
    }
  }
}