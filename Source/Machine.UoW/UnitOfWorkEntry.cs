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