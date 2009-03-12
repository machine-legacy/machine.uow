using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkScope : IDisposable
  {
    void Add(Type key, IScopeProvider provider);
    T Get<T>(T defaultValue) where T : IDisposable;
    T Get<T>() where T : IDisposable;
    void Set<T>(T value) where T : IDisposable;
    void Set(Type key, IDisposable value);
  }

  public class NullScope : IUnitOfWorkScope
  {
    public void Dispose()
    {
    }

    public void Add(Type key, IScopeProvider provider)
    {
    }

    public T Get<T>(T defaultValue) where T : IDisposable
    {
      return defaultValue;
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
  }
}