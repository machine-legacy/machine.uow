using System;
using System.Collections.Generic;

using Machine.Specifications;

using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  [Subject("Adding objects")]
  public class when_adding_new_object : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;

    Because of = () =>
    {
      using (IUnitOfWork uow = UoW.Start())
      {
        NorthwindEmployee employee = new NorthwindEmployee();
        employee.FirstName = "Steve";
        employee.LastName = "Zandt";
        employee.BirthDate = DateTime.Now;
        employee.IsAlive = true;
        uow.AddNew(employee);
        uow.Commit();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employees = uow.Session().CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_not_be_saved = () =>
      employees.ShouldNotBeEmpty();
  }
  
  [Subject("Adding objects")]
  public class when_adding_new_object_and_rolling_back : with_nhibernate_uow
  {
    static ICollection<NorthwindEmployee> employees;

    Because of = () =>
    {
      using (IUnitOfWork uow = UoW.Start())
      {
        NorthwindEmployee employee = new NorthwindEmployee();
        employee.FirstName = "Steve";
        employee.LastName = "Zandt";
        employee.BirthDate = DateTime.Now;
        employee.IsAlive = true;
        uow.AddNew(employee);
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employees = uow.Session().CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_not_be_saved = () =>
      employees.ShouldBeEmpty();
  }
}
