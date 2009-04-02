using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkScope : IDisposable
  {
    void Add(Type key, IScopeProvider provider);
    T Get<T>(T defaultValue) where T : IDisposable;
    T Get<T>(Func<T> factory) where T : IDisposable;
    T Get<T>() where T : IDisposable;
    void Set<T>(T value) where T : IDisposable;
    void Set(Type key, IDisposable value);
    void Remove<T>();

    event EventHandler<EventArgs> Disposed;
  }

  public class NullScope : IUnitOfWorkScope
  {
    public static readonly IUnitOfWorkScope Null = new NullScope();

    public void Dispose()
    {
      Disposed(this, EventArgs.Empty);
    }

    public void Add(Type key, IScopeProvider provider)
    {
    }

    public T Get<T>(T defaultValue) where T : IDisposable
    {
      return defaultValue;
    }

    public T Get<T>(Func<T> factory) where T : IDisposable
    {
      return default(T);
    }

    public T Get<T>() where T : IDisposable
    {
      return default(T);
    }

    public void Set<T>(T value) where T : IDisposable
    {
      throw new NotSupportedException();
    }

    public void Set(Type key, IDisposable value)
    {
      throw new NotSupportedException();
    }

    public void Remove<T>()
    {
      throw new NotSupportedException();
    }

    public event EventHandler<EventArgs> Disposed;
  }
}