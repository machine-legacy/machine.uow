using System;
using System.Collections.Generic;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;
using NHibernate;

namespace Machine.UoW.NHibernate.Specs
{
  [Subject("Querying")]
  public class when_querying_for_data : with_sqlite_database
  {
    static ICollection<NorthwindEmployee> employees;
    static ISession session;

    Establish context = () =>
    {
      session = database.SessionFactory.OpenSession();
      database.AddDefaultData();
    };

    Cleanup after = () =>
      session.Dispose();

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
      using (var uow = SpecUoW.Start())
      using (var session = SpecUoW.OpenSession())
      {
        employees = NH.Session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_return_objects = () =>
      employees.ShouldNotBeEmpty();
  }
}
