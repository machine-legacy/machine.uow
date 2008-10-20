using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly Dictionary<object, UnitOfWorkEntry> _entries = new Dictionary<object, UnitOfWorkEntry>();
    private readonly IUnitOfWorkManagement _unitOfWorkManagement;
    private bool _open;

    public UnitOfWork(IUnitOfWorkManagement unitOfWorkManagement)
    {
      _unitOfWorkManagement = unitOfWorkManagement;
      _open = true;
    }

    public void Start()
    {
      _unitOfWorkManagement.GetUnitOfWorkEventsProxy().Start(this);
    }

    public void AddNew<T>(T instance)
    {
      AssertIsOpen();
      FindOrCreateEntryFor(instance).AddNew();
    }

    public void Save<T>(T instance)
    {
      AssertIsOpen();
      FindOrCreateEntryFor(instance).Save();
    }

    public void Remove<T>(T instance)
    {
      AssertIsOpen();
      if (!HasEntryFor(instance))
      {
        throw new InvalidOperationException("Not allowed to remove instance that is NOT in the UoW.");
      }
      _entries.Remove(instance);
    }

    public void Delete<T>(T instance)
    {
      AssertIsOpen();
      FindOrCreateEntryFor(instance).Delete();
    }

    public void Commit()
    {
      Close();
      IUnitOfWorkEvents events = _unitOfWorkManagement.GetUnitOfWorkEventsProxy();
      foreach (UnitOfWorkEntry entry in _entries.Values)
      {
        entry.Commit(this, events);
      }
      events.Commit(this);
      _entries.Clear();
    }

    public void Rollback()
    {
      Close();
      IUnitOfWorkEvents events = _unitOfWorkManagement.GetUnitOfWorkEventsProxy();
      foreach (UnitOfWorkEntry entry in _entries.Values)
      {
        entry.Rollback(this, events);
      }
      events.Rollback(this);
      _entries.Clear();
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

    public void Dispose()
    {
      Commit();
    }

    private void AssertIsOpen()
    {
      if (!_open)
      {
        throw new InvalidOperationException("This UoW is closed! It has been committed or rolled back!");
      }
    }

    private void Close()
    {
      AssertIsOpen();
      _open = false;
      this.Closed(this, EventArgs.Empty);
    }

    public event EventHandler<EventArgs> Closed = delegate(object sender, EventArgs e) { };

    private readonly Dictionary<Type, object> _state = new Dictionary<Type, object>();

    #region IUnitOfWorkState Members
    public T Get<T>()
    {
      if (!_state.ContainsKey(typeof(T)))
      {
        return default(T);
      }
      return (T)_state[typeof(T)];
    }

    public void Set<T>(T value)
    {
      _state[typeof(T)] = value;
    }
    #endregion
  }
}