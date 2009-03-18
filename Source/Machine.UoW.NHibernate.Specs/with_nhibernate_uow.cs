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
      unitOfWorkManagement.AddEvents(new NHibernateScopeEvents(database.SessionFactory));
      unitOfWorkManagement.AddEvents(new NHibernateUoWEvents());
      IUnitOfWorkFactory factory = new UnitOfWorkFactory(unitOfWorkManagement);
      IUnitOfWorkScopeProvider unitOfWorkScopeProvider = new ThreadStaticUnitOfWorkScopeProvider(factory);
      UoW.Startup(new HybridUnitOfWorkProvider(factory), unitOfWorkScopeProvider, new NHibernateTransactionProvider(unitOfWorkScopeProvider));
    };
  }
}