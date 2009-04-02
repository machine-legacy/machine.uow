using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.UoW
{
  public class UnitOfWorkScope : IUnitOfWorkScope
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UnitOfWorkScope));
    readonly IDictionary<Type, IScopeProvider> _providers = new Dictionary<Type, IScopeProvider>();
    readonly IDictionary<Type, IDisposable> _state = new Dictionary<Type, IDisposable>();
    readonly List<IDisposable> _additions = new List<IDisposable>();
    readonly IUnitOfWorkScope _parentScope;

    public UnitOfWorkScope(IUnitOfWorkScope parentScope)
    {
      _parentScope = parentScope;
    }

    public void Add(Type key, IScopeProvider provider)
    {
      _providers[key] = provider;
    }

    public T Get<T>(T defaultValue) where T : IDisposable
    {
      Type key = typeof(T);
      if (!_state.ContainsKey(key))
      {
        if (_providers.ContainsKey(key))
        {
          _log.Debug("Invoking Provider: " + key);
          Set(key, _providers[key].Create(this));
        }
        else
        {
          return _parentScope.Get(defaultValue);
        }
      }
      return (T)_state[key];
    }

    public T Get<T>(Func<T> factory) where T : IDisposable
    {
      Type key = typeof(T);
      if (!_state.ContainsKey(key))
      {
        T value = factory();
        Set(key, value);
      }
      return (T)_state[key];
    }

    public T Get<T>() where T : IDisposable
    {
      return Get(default(T));
    }

    public void Set(Type key, IDisposable value)
    {
      if (value == null) throw new ArgumentException(key + " has NULL scope value", "value");
      _state[key] = value;
      _additions.Add(value);
    }

    public void Set<T>(T value) where T : IDisposable
    {
      Set(typeof(T), value);
    }

    public void Remove<T>()
    {
      _state.Remove(typeof(T));
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