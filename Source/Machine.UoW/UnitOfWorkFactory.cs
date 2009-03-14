using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkFactory : IUnitOfWorkFactory
  {
    private readonly IUnitOfWorkManagement _unitOfWorkManagement;

    public UnitOfWorkFactory(IUnitOfWorkManagement unitOfWorkManagement)
    {
      _unitOfWorkManagement = unitOfWorkManagement;
    }

    public IUnitOfWork StartUnitOfWork(IUnitOfWorkScope scope)
    {
      UnitOfWork unitOfWork = new UnitOfWork(_unitOfWorkManagement, scope);
      unitOfWork.Start();
      return unitOfWork;
    }

    public IUnitOfWorkScope StartScope(IUnitOfWorkSettings[] allSettings)
    {
      UnitOfWorkScope scope = new UnitOfWorkScope(NullScope.Null);
      foreach (IUnitOfWorkSettings settings in allSettings)
      {
        scope.Set(settings.GetType(), settings);
      }
      _unitOfWorkManagement.GetScopeEventsProxy().Start(scope);
      return scope;
    }
  }
}
