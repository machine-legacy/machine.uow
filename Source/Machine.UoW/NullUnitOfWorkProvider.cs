using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class NullUnitOfWorkProvider : IUnitOfWorkProvider
  {
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public NullUnitOfWorkProvider()
      : this(new NullScopeProvider())
    {
    }

    public NullUnitOfWorkProvider(IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IUnitOfWork Start(IUnitOfWorkScope scope, params IUnitOfWorkSettings[] settings)
    {
      return new NullUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope());
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return new NullUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope());
    }
  }
}