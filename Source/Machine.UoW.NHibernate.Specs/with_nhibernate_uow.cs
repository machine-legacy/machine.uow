using System;
using System.Collections.Generic;

using Machine.Specifications;
using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_nhibernate_uow : with_sqlite_database
  {
    protected static Exception exception;

    Establish context = () =>
    {
      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new AdoNetConnectionScopeEvents(SqlHelper.Provider));
      unitOfWorkManagement.AddEvents(new NHibernateUoWEvents(database.SessionFactory));
      IUnitOfWorkFactory factory = new UnitOfWorkFactory(unitOfWorkManagement);
      UoW.Provider = new HybridUnitOfWorkProvider(factory);
    };
  }
}