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
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
        employee.FirstName = "Steve Van";
        uow.Commit();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
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
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
        employee.FirstName = "Steve Van";
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_unchanged_in_database = () =>
      employee.FirstName.ShouldNotEqual("Steve Van");
  }

  [Subject("Saving objects")]
  public class when_saving_an_object_in_transaction_scope_with_no_complete : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope())
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
        }
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_unchanged_in_database = () =>
      employee.FirstName.ShouldEqual("Steve");
  }

  [Subject("Saving objects")]
  public class when_saving_an_object_in_transaction_scope : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope())
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          uow.Commit();
        }
        scope.Complete();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Saving objects")]
  public class when_saving_an_object_in_transaction_scope_and_no_commit : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope())
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
        }
        scope.Complete();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_not_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve");
  }
}
