using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly Dictionary<object, UnitOfWorkEntry> _entries = new Dictionary<object, UnitOfWorkEntry>();
    private readonly IUnitOfWorkManagement _unitOfWorkManagement;

    public UnitOfWork(IUnitOfWorkManagement unitOfWorkManagement)
    {
      _unitOfWorkManagement = unitOfWorkManagement;
    }

    public void AddNew<T>(T instance)
    {
      FindOrCreateEntryFor(instance).Add();
    }

    public void Save<T>(T instance)
    {
      FindOrCreateEntryFor(instance).Save();
    }

    public void Remove<T>(T instance)
    {
      if (!HasEntryFor(instance))
      {
        throw new InvalidOperationException("Not allowed to remove instance that is NOT in the UoW.");
      }
      _entries.Remove(instance);
    }

    public void Delete<T>(T instance)
    {
      FindOrCreateEntryFor(instance).Delete();
    }

    public void Commit()
    {
      foreach (UnitOfWorkEntry entry in _entries.Values)
      {
        entry.Commit(_unitOfWorkManagement);
      }
    }

    public void Rollback()
    {
      foreach (UnitOfWorkEntry entry in _entries.Values)
      {
        entry.Rollback(_unitOfWorkManagement);
      }
    }

    public IEnumerable<UnitOfWorkEntry> Entries
    {
      get { return _entries.Values; }
    }

    public UnitOfWorkEntry FindEntryFor(object instance)
    {
      if (HasEntryFor(instance))
      {
        return _entries[instance];
      }
      throw new KeyNotFoundException();
    }

    private UnitOfWorkEntry FindOrCreateEntryFor(object instance)
    {
      if (!HasEntryFor(instance))
      {
        _entries[instance] = new UnitOfWorkEntry(instance);
      }
      return _entries[instance];
    }

    public bool HasEntryFor(object instance)
    {
      return _entries.ContainsKey(instance);
    }
  }
}