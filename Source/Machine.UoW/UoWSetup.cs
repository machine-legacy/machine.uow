using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public static class UoWSetup
  {
    public static void NullThreadStaticUoW()
    {
      UoW.Provider = new ThreadStaticUnitOfWorkProvider(NullFactory());
      UoW.ScopeProvider = new NullScopeProvider();
    }

    public static void NullHttpUoW()
    {
      UoW.Provider = new HttpContextUnitOfWorkProvider(NullFactory());
      UoW.ScopeProvider = new NullScopeProvider();
    }

    public static IUnitOfWorkFactory NullFactory()
    {
      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new NullUnitOfWorkEvents());
      return new UnitOfWorkFactory(unitOfWorkManagement);
    }
  }

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
      return new NullUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope(settings));
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return new NullUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope());
    }
  }
  
  public class NullScopeProvider : IUnitOfWorkScopeProvider
  {
    public IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings)
    {
      return NullScope.Null;
    }
  }
}