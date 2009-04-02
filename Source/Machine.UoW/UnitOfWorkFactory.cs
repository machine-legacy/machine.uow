using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkFactory : IUnitOfWorkFactory
  {
    readonly IUnitOfWorkManagement _unitOfWorkManagement;
    readonly IUnitOfWorkScopeFactory _unitOfWorkScopeFactory;

    public UnitOfWorkFactory(IUnitOfWorkManagement unitOfWorkManagement)
    {
      _unitOfWorkManagement = unitOfWorkManagement;
      _unitOfWorkScopeFactory = new UnitOfWorkScopeFactory();
    }

    public IUnitOfWork StartUnitOfWork(IUnitOfWorkScope scope)
    {
      UnitOfWork unitOfWork = new UnitOfWork(_unitOfWorkManagement, scope);
      unitOfWork.Start();
      return unitOfWork;
    }

    public IUnitOfWorkScope StartScope(IUnitOfWorkScope parentScope, IUnitOfWorkSettings[] allSettings)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeFactory.StartScope(parentScope, allSettings);
      _unitOfWorkManagement.GetScopeEventsProxy().Start(scope);
      return scope;
    }
  }

  public class UnitOfWorkScopeFactory : IUnitOfWorkScopeFactory
  {
    public IUnitOfWorkScope StartScope(IUnitOfWorkSettings[] allSettings)
    {
      return StartScope(NullScope.Null, allSettings);
    }

    public IUnitOfWorkScope StartScope(IUnitOfWorkScope parentScope, IUnitOfWorkSettings[] allSettings)
    {
      UnitOfWorkScope scope = new UnitOfWorkScope(parentScope);
      foreach (IUnitOfWorkSettings settings in allSettings)
      {
        scope.Set(settings.GetType(), settings);
      }
      return scope;
    }
  }
}
