using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWork : IUnitOfWorkState, IDisposable
  {
    void AddNew<T>(T instance);
    void Save<T>(T instance);
    void Remove<T>(T instance);
    void Delete<T>(T instance);
    void Commit();
    void Rollback();
    event EventHandler<EventArgs> Closed;
  }
  public interface IUnitOfWorkState
  {
    T Get<T>();
    void Set<T>(T value);
  }
}
