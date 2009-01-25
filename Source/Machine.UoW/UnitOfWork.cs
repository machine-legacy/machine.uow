using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public enum CommitOrRollbackType
  {
    Local,
    Ambient
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly Dictionary<object, UnitOfWorkEntry> _entries = new Dictionary<object, UnitOfWorkEntry>();
    private readonly IUnitOfWorkManagement _unitOfWorkManagement;
    private bool _open;
    private bool _disposed;

    public UnitOfWork(IUnitOfWorkManagement unitOfWorkManagement, params IUnitOfWorkSettings[] startupSettings)
    {
      // EnlistmentNotifications.Enlist(this);
      _unitOfWorkManagement = unitOfWorkManagement;
      _open = true;
      foreach (IUnitOfWorkSettings settings in startupSettings)
      {
        _state[settings.GetType()] = settings;
      }
    }

    public bool IsClosed
    {
      get { return !_open; }
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
      Commit(CommitOrRollbackType.Local);
    }

    public void Commit(CommitOrRollbackType type)
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
      Rollback(CommitOrRollbackType.Local);
    }

    public void Rollback(CommitOrRollbackType type)
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
      if (_open)
      {
        Rollback();
      }
      if (_disposed)
      {
        return;
      }
      IUnitOfWorkEvents events = _unitOfWorkManagement.GetUnitOfWorkEventsProxy();
      events.Dispose(this);
      _disposed = true;
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
    public T Get<T>(T defaultValue)
    {
      if (!_state.ContainsKey(typeof(T)))
      {
        return defaultValue;
      }
      return (T)_state[typeof(T)];
    }

    public T Get<T>()
    {
      return Get<T>(default(T));
    }

    public void Set<T>(T value)
    {
      _state[typeof(T)] = value;
    }
    #endregion
  }
}