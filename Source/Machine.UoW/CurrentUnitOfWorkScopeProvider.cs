using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class CurrentUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkProvider _unitOfWorkProvider;

    public CurrentUnitOfWorkScopeProvider(IUnitOfWorkProvider unitOfWorkProvider)
    {
      _unitOfWorkProvider = unitOfWorkProvider;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope()
    {
      IUnitOfWork unitOfWork = _unitOfWorkProvider.GetUnitOfWork();
      if (unitOfWork == null)
      {
        throw new InvalidOperationException("No current UnitOfWork");
      }
      return unitOfWork.Scope;
    }
  }
}
