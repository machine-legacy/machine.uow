using System;
using System.Collections.Generic;

namespace Machine.UoW.AdoDotNet
{
  public class AdoNetConnectionUoWEvents : IUnitOfWorkEvents
  {
    readonly IConnectionProvider _connectionProvider;

    public AdoNetConnectionUoWEvents(IConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    public void Start(IUnitOfWork unitOfWork)
    {
      unitOfWork.Scope.Set(new CurrentConnection(_connectionProvider));
    }

    public void AddNew(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Save(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Delete(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Rollback(IUnitOfWork unitOfWork, object obj)
    {
    }

    public void Rollback(IUnitOfWork unitOfWork)
    {
    }

    public void Commit(IUnitOfWork unitOfWork)
    {
    }

    public void Dispose(IUnitOfWork unitOfWork)
    {
      unitOfWork.Scope.Get<CurrentConnection>().Close();
    }
  }
}
