using System;
using System.Collections.Generic;
using System.Transactions;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs.AmbientTransactions
{
  [Subject("Ambient Transactions")]
  public class when_saving_an_object : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (TransactionScope txn = new TransactionScope())
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          uow.Commit();
        }
        txn.Complete();
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Ambient Transactions")]
  public class when_saving_an_object_and_not_committing : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (TransactionScope txn = new TransactionScope())
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          uow.Commit();
        }
      }
      using (IUnitOfWork uow = UoW.Start())
      {
        employee = uow.Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_not_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve");
  }

  [Subject("Ambient Transactions")]
  public class when_saving_an_object_and_an_error_occurs : with_nhibernate_uow
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      try
      {
        using (TransactionScope txn = new TransactionScope())
        {
          using (IUnitOfWork uow = UoW.Start())
          {
            employee = uow.Session().Get<NorthwindEmployee>(id);
            employee.FirstName = "Steve Van";
            throw new InvalidOperationException();
          }
        }
      }
      catch
      {
        using (IUnitOfWork uow = UoW.Start())
        {
          employee = uow.Session().Get<NorthwindEmployee>(id);
          uow.Commit();
        }
      }
    };

    It should_not_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve");
  }
}
