using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class CurrentScopeProvider : IUnitOfWorkProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public CurrentScopeProvider(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IUnitOfWork Start(IUnitOfWorkScope scope, params IUnitOfWorkSettings[] settings)
    {
      if (scope.Get<IUnitOfWorkScope>() != null)
      {
        throw new InvalidOperationException("Nesting of Units of Work is forbidden");
      }
      IUnitOfWork unitOfWork = _unitOfWorkFactory.StartUnitOfWork(scope);
      unitOfWork.Closed += OnClosed;
      scope.Set(unitOfWork);
      return unitOfWork;
    }

    private void OnClosed(object sender, EventArgs e)
    {
      _unitOfWorkScopeProvider.GetUnitOfWorkScope().Remove<IUnitOfWork>();
    }

    public IUnitOfWork GetUnitOfWork()
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      return scope.Get<IUnitOfWork>();
    }
  }
}
