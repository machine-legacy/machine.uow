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
}