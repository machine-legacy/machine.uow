using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkEntry
  {
    private readonly object _instance;
    private readonly List<UnitOfWorkChange> _changes;

    public object Instance
    {
      get { return _instance; }
    }

    public IEnumerable<UnitOfWorkChange> Changes
    {
      get { return _changes; }
    }

    public UnitOfWorkEntry(object instance, List<UnitOfWorkChange> changes)
    {
      _instance = instance;
      _changes = changes;
    }

    public void Saved()
    {
    }

    public void Deleted()
    {
    }

    public void Updated()
    {
    }
  }
}