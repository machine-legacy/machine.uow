using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class NullUnitOfWork : IUnitOfWork
  {
    readonly IUnitOfWorkScope _scope;

    public NullUnitOfWork()
      : this(NullScope.Null)
    {
    }

    public NullUnitOfWork(IUnitOfWorkScope scope)
    {
      _scope = scope;
    }

    public void Dispose()
    {
      this.Closed(this, EventArgs.Empty);
    }

    public bool IsClosed
    {
      get { return false; }
    }

    public IUnitOfWorkScope Scope
    {
      get { return _scope; }
    }

    public bool WasCommitted
    {
      get { return false; }
    }

    public bool WasRolledBack
    {
      get { return false; }
    }

    public void AddNew<T>(T instance)
    {
    }

    public void Save<T>(T instance)
    {
    }

    public void Remove<T>(T instance)
    {
    }

    public void Delete<T>(T instance)
    {
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public event EventHandler<EventArgs> Closed = delegate(object sender, EventArgs e) { };
  }
}
