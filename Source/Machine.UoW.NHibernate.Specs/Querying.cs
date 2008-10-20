using System;
using System.Collections.Generic;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  [Subject("Querying")]
  public class when_querying_for_data : with_sqlite_database
  {
    static ICollection<NorthwindEmployee> employees;

    Establish context = () => database.AddDefaultData();

    Because of = () =>
      employees = session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();

    It should_return_objects = () =>
      employees.ShouldNotBeEmpty();
  }
  
  [Subject("Querying")]
  public class when_querying_with_in_a_uow : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;

    Establish context = () => database.AddDefaultData();

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
