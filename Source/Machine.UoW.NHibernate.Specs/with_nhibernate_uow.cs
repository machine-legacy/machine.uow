using System;
using System.Collections.Generic;

using Machine.UoW.NHibernate.Specs.NorthwindModel;

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

  [Subject("Querying")]
  public class when_querying_with_a_valid_uow : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;

    Because of = () =>
    {
      using (IUnitOfWork uow = UoW.Start())
      {
        employees = uow.Session().CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_return_objects = () =>
      employees.ShouldNotBeEmpty();
  }
}