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
      using (IUnitOfWork uow = UoW.Start())
      {
        uow.Delete(uow.Session().Get<NorthwindEmployee>(id));
        uow.Commit();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employees = uow.Session().CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_not_be_saved = () =>
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
      using (IUnitOfWork uow = UoW.Start())
      {
        uow.Delete(uow.Session().Get<NorthwindEmployee>(id));
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employees = uow.Session().CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_not_be_saved = () =>
      employees.ShouldNotBeEmpty();
  }
}
