using System;
using System.Collections.Generic;

using Machine.UoW.NHibernate.Specs.NorthwindModel;

using Machine.Specifications;

namespace Machine.UoW.NHibernate.Specs.TransactionSpecs
{
  [Subject("Session Transactions")]
  public class when_using_implicit_transaction : TransactionSpecs
  {
    static ICollection<NorthwindEmployee> employees;

    Establish context = () => database.AddDefaultData();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        employees = NH.Session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();
      }
    };

    It should_return_objects = () =>
      employees.ShouldNotBeEmpty();
  }

  [Subject("Session Transactions")]
  public class when_using_implicit_transaction_explicitly : TransactionSpecs
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        using (var transaction = session.Begin())
        {
          var employee = NH.Session.Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          transaction.Commit();
        }
      }
      using (SpecDatabase.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Session Transactions")]
  public class when_using_implicit_transaction_explicitly_in_scopes_and_rolling_back : TransactionSpecs
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        using (var transaction = session.Begin())
        {
          var employee = NH.Session.Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
        }
      }
      using (SpecDatabase.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_unchanged_in_database = () =>
      employee.FirstName.ShouldNotEqual("Steve Van");
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_in_scopes : TransactionSpecs
  {
    static NorthwindEmployee employee1;
    static NorthwindEmployee employee2;
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        using (var transaction = session.Begin())
        {
          employee1 = NH.Session.Get<NorthwindEmployee>(id);
          employee1.FirstName = "Steve Van #1";
          transaction.Commit();
        }
        using (var transaction = session.Begin())
        {
          employee2 = NH.Session.Get<NorthwindEmployee>(id);
          employee2.FirstName = "Steve Van #2";
          transaction.Commit();
        }
      }
      using (SpecDatabase.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van #2");

    It should_have_gotten_the_same_employee = () =>
      employee1.ShouldEqual(employee2);
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_in_scopes_and_rolling_one_back : TransactionSpecs
  {
    static NorthwindEmployee employee1;
    static NorthwindEmployee employee2;
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        using (var transaction = session.Begin())
        {
          employee1 = NH.Session.Get<NorthwindEmployee>(id);
          employee1.FirstName = "Jacob Lewallen";
        }
        using (var transaction = session.Begin())
        {
          employee2 = NH.Session.Get<NorthwindEmployee>(id);
          employee2.FirstName = "Steve Van";
          transaction.Commit();
        }
      }
      using (SpecDatabase.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");

    It should_have_gotten_the_same_employee = () =>
      employee1.ShouldEqual(employee2);
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_and_rolling_one_back : TransactionSpecs
  {
    static NorthwindEmployee employee;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        session.Begin();
        var employee = NH.Session.Get<NorthwindEmployee>(id);
        employee.FirstName = "Jacob Lewallen";
        session.Rollback();

        session.Begin();
        employee = NH.Session.Get<NorthwindEmployee>(id);
        employee.FirstName = "Steve Van";
        session.Commit();
      }
      using (SpecDatabase.OpenSession())
      {
        employee = NH.Session.Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_and_attempting_to_nest_them : TransactionSpecs
  {
    static Exception error;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        session.Begin();
        error = Catch.Exception(() => session.Begin());
      }
    };

    It should_throw_exception = () =>
      error.ShouldNotBeNull();
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_and_attempting_to_commit_twice : TransactionSpecs
  {
    static Exception error;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        session.Begin();
        session.Commit();
        error = Catch.Exception(session.Commit);
      }
    };

    It should_throw_exception = () =>
      error.ShouldNotBeNull();
  }

  [Subject("Session Transactions")]
  public class when_using_multiple_transactions_and_attempting_to_begin_from_a_transaction_itself : TransactionSpecs
  {
    static Exception error;
    static long id;

    Establish context = () => id = database.AddSingleEmployee();

    Because of = () =>
    {
      using (var session = SpecDatabase.OpenSession())
      {
        using (var transaction = session.Begin())
        {
          error = Catch.Exception(() => transaction.Begin());
        }
      }
    };

    It should_throw_exception = () =>
      error.ShouldNotBeNull();
  }

  public class TransactionSpecs : with_nhibernate_uow
  {
  }
}
