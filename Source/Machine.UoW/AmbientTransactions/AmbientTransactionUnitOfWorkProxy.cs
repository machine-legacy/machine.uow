using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUnitOfWorkProxy : IUnitOfWork
  {
    [ThreadStatic]
    static AmbientTransactionUnitOfWorkProxy _active;
    readonly IUnitOfWork _unitOfWork;
    readonly TransactionScope _scope;

    public static AmbientTransactionUnitOfWorkProxy Active
    {
      get { return _active; }
    }

    public AmbientTransactionUnitOfWorkProxy(IUnitOfWork unitOfWork, TransactionScope scope)
    {
      _unitOfWork = unitOfWork;
      _scope = scope;
      _active = this;
    }

    public bool WasCommitted { get { return _unitOfWork.WasCommitted; } }

    public bool WasRolledBack { get { return _unitOfWork.WasRolledBack; } }
    
    public T Get<T>(T defaultValue) where T : IDisposable
    {
      return _unitOfWork.Get(defaultValue);
    }

    public T Get<T>() where T : IDisposable
    {
      return _unitOfWork.Get<T>();
    }

    public void Set<T>(T value) where T : IDisposable
    {
      _unitOfWork.Set(value);
    }

    public bool IsClosed
    {
      get { return _unitOfWork.IsClosed; }
    }

    public void AddNew<T>(T instance)
    {
      _unitOfWork.AddNew(instance);
    }

    public void Save<T>(T instance)
    {
      _unitOfWork.Save(instance);
    }

    public void Remove<T>(T instance)
    {
      _unitOfWork.Remove(instance);
    }

    public void Delete<T>(T instance)
    {
      _unitOfWork.Delete(instance);
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public void Dispose()
    {
      _active = null;
      _scope.Dispose();
    }

    public event EventHandler<EventArgs> Closed
    {
      add { throw new InvalidOperationException(); }
      remove { throw new InvalidOperationException(); }
    }
  }
}