using System;
using System.Collections.Generic;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  [Subject("Deleting objects")]
  public class when_deleting_an_object : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var uow = SpecDatabase.Start())
      using (var session = SpecDatabase.OpenSession())
      {
        uow.Delete(NH.Session.Get<NorthwindEmployee>(id));
        uow.Commit();
        session.Commit();
      }
      using (var uow = SpecDatabase.Start())
      using (var session = SpecDatabase.OpenSession())
      {
        employees = NH.Session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_disappear_from_database = () =>
      employees.ShouldBeEmpty();
  }
  
  [Subject("Deleting objects")]
  public class when_deleting_an_object_and_rolling_back : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var uow = SpecDatabase.Start())
      using (var session = SpecDatabase.OpenSession())
      {
        uow.Delete(NH.Session.Get<NorthwindEmployee>(id));
      }
      using (var uow = SpecDatabase.Start())
      using (var session = SpecDatabase.OpenSession())
      {
        employees = NH.Session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_appear_in_database = () =>
      employees.ShouldNotBeEmpty();
  }
}
