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
}