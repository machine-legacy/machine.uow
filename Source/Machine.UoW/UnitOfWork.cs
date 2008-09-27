using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly Dictionary<object, UnitOfWorkEntry> _entries = new Dictionary<object, UnitOfWorkEntry>();

    public void AddNew<T>(T instance)
    {
      throw new System.NotImplementedException();
    }

    public void Save<T>(T instance)
    {
      throw new System.NotImplementedException();
    }

    public void Remove<T>(T instance)
    {
      throw new System.NotImplementedException();
    }

    public void Delete<T>(T instance)
    {
      throw new System.NotImplementedException();
    }

    public void Commit()
    {
      throw new System.NotImplementedException();
    }

    public void Rollback()
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<UnitOfWorkEntry> Entries
    {
      get { return _entries.Values; }
    }

    public UnitOfWorkEntry FindEntryFor(object instance)
    {
      throw new System.NotImplementedException();
    }
  }
}