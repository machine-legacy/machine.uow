using System;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_nhibernate_uow : with_sqlite_database
  {
    Establish context = () =>
    {
      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new NHibernateUoWEvents(database.SessionFactory));
      IUnitOfWorkFactory factory = new UnitOfWorkFactory(unitOfWorkManagement);
      UoW.Provider = new ThreadStaticUnitOfWorkProvider(factory);
    };
  }
}