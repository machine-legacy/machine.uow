using System;
using System.Collections.Generic;
using System.Transactions;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  [Subject("Saving objects")]
  public class when_saving_an_object : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var uow = SpecUoW.Start())
      using (var session = SpecUoW.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
        employee.FirstName = "Steve Van";
        session.Commit();
      }
      using (var uow = SpecUoW.Start())
      using (var session = SpecUoW.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }
  
  [Subject("Saving objects")]
  public class when_saving_an_object_and_rolling_back : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var uow = SpecUoW.Start())
      using (var session = SpecUoW.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
        employee.FirstName = "Steve Van";
      }
      using (var uow = SpecUoW.Start())
      using (var session = SpecUoW.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_unchanged_in_database = () =>
      employee.FirstName.ShouldNotEqual("Steve Van");
  }
}
