using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public enum CommitOrRollbackType
  {
    Local,
    Ambient
  }
  
  public class UnitOfWork : UnitOfWorkScopeBase, IUnitOfWork
  {
    private readonly Dictionary<object, UnitOfWorkEntry> _entries = new Dictionary<object, UnitOfWorkEntry>();
    private readonly IUnitOfWorkManagement _unitOfWorkManagement;
    private bool _open;
    private bool _disposed;
    private bool _wasCommitted;

    public UnitOfWork(IUnitOfWorkManagement unitOfWorkManagement, params IUnitOfWorkSettings[] startupSettings)
    {
      _unitOfWorkManagement = unitOfWorkManagement;
      _open = true;
      foreach (IUnitOfWorkSettings settings in startupSettings)
      {
        Set(settings.GetType(), settings);
      }
    }

    public bool IsClosed
    {
      get { return !_open; }
    }

    public bool WasCommitted
    {
      get { return !_open && _wasCommitted; }
    }

    public bool WasRolledBack
    {
      get { return !_open && !_wasCommitted; }
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
      _wasCommitted = true;
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

    public override void Dispose()
    {
      base.Dispose();
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
  }

  public abstract class UnitOfWorkScopeBase : IUnitOfWorkScope
  {
    readonly Dictionary<Type, IDisposable> _state = new Dictionary<Type, IDisposable>();

    public T Get<T>(T defaultValue) where T : IDisposable
    {
      if (!_state.ContainsKey(typeof(T)))
      {
        return defaultValue;
      }
      return (T)_state[typeof(T)];
    }

    public T Get<T>() where T : IDisposable
    {
      return Get<T>(default(T));
    }

    public void Set(Type key, IDisposable value)
    {
      _state[key] = value;
    }

    public void Set<T>(T value) where T : IDisposable
    {
      Set(typeof(T), value);
    }

    public virtual void Dispose()
    {
    }
  }
}