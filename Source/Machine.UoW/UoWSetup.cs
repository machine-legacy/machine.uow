using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public static class UoWSetup
  {
    public static void NullThreadStaticUoW()
    {
      // UoW.Startup(new ThreadStaticUnitOfWorkProvider(NullFactory()), new NullScopeProvider(), new NullTransactionProvider());
    }

    public static void NullHttpUoW()
    {
      // UoW.Startup(new HttpContextUnitOfWorkProvider(NullFactory()), new NullScopeProvider(), new NullTransactionProvider());
    }

    public static IUnitOfWorkFactory NullFactory()
    {
      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new NullUnitOfWorkEvents());
      return new UnitOfWorkFactory(unitOfWorkManagement);
    }
  }
}