using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.UoW
{
  public class UnitOfWorkScope : IUnitOfWorkScope
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UnitOfWorkScope));
    readonly IDictionary<object, IScopeProvider> _providers = new Dictionary<object, IScopeProvider>();
    readonly IDictionary<object, IDisposable> _state = new Dictionary<object, IDisposable>();
    readonly List<IDisposable> _additions = new List<IDisposable>();
    readonly IUnitOfWorkScope _parentScope;

    public UnitOfWorkScope(IUnitOfWorkScope parentScope)
    {
      _parentScope = parentScope;
    }

    public void Add(object key, IScopeProvider provider)
    {
      _providers[key] = provider;
    }

    public T Get<T>(object key, T defaultValue) where T : IDisposable
    {
      if (!_state.ContainsKey(key))
      {
        if (_providers.ContainsKey(key))
        {
          _log.Debug("Invoking Provider: " + key);
          Set(key, _providers[key].Create(this));
        }
        else
        {
          return _parentScope.Get<T>(key, defaultValue);
        }
      }
      return (T)_state[key];
    }

    public T Get<T>(object key, Func<T> factory) where T : IDisposable
    {
      if (!_state.ContainsKey(key))
      {
        T value = factory();
        Set(key, value);
      }
      return (T)_state[key];
    }

    public T Get<T>(object key) where T : IDisposable
    {
      return Get(key, default(T));
    }

    public void Set(object key, IDisposable value)
    {
      if (value == null) throw new ArgumentException(key + " has NULL scope value", "value");
      _state[key] = value;
      _additions.Add(value);
    }

    public void Set<T>(object key, T value) where T : IDisposable
    {
      Set(key, (IDisposable)value);
    }

    public void Remove(object key)
    {
      _state.Remove(key);
    }

    public virtual void Dispose()
    {
      IEnumerable<IDisposable> order = _additions;
      foreach (IDisposable disposable in order.Reverse())
      {
        _log.Debug("Disposing: " + disposable);
        disposable.Dispose();
      }
      Disposed(this, EventArgs.Empty);
    }

    public event EventHandler<EventArgs> Disposed = delegate(object sender, EventArgs e) { };
  }
}