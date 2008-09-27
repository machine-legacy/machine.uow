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
  }
}