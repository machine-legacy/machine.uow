using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkScope : IDisposable
  {
    void Add(object key, IScopeProvider provider);
    T Get<T>(object key, T defaultValue) where T : IDisposable;
    T Get<T>(object key, Func<T> factory) where T : IDisposable;
    T Get<T>(object key) where T : IDisposable;
    void Set<T>(object key, T value) where T : IDisposable;
    void Set(object key, IDisposable value);
    void Remove(object key);

    event EventHandler<EventArgs> Disposed;
  }

  public class NullScope : IUnitOfWorkScope
  {
    public static readonly IUnitOfWorkScope Null = new NullScope();

    public void Dispose()
    {
      Disposed(this, EventArgs.Empty);
    }

    public void Add(object key, IScopeProvider provider)
    {
    }

    public T Get<T>(object key, T defaultValue) where T : IDisposable
    {
      return defaultValue;
    }

    public T Get<T>(object key, Func<T> factory) where T : IDisposable
    {
      return default(T);
    }

    public T Get<T>(object key) where T : IDisposable
    {
      return default(T);
    }

    public void Set<T>(object key, T value) where T : IDisposable
    {
      throw new NotSupportedException();
    }

    public void Set(object key, IDisposable value)
    {
      throw new NotSupportedException();
    }

    public void Remove(object key)
    {
      throw new NotSupportedException();
    }

    public event EventHandler<EventArgs> Disposed;
  }
}