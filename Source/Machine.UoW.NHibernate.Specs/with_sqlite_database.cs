using System;
using System.Collections.Generic;

using NHibernate;

using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_sqlite_database
  {
    protected static DatabaseViaNHibernate database;
    protected static ISession session;

    Establish context = () =>
    {
      database = new DatabaseViaNHibernate();
      session = database.SessionFactory.OpenSession();
    };
  }

  [Subject("Querying")]
  public class when_querying_for_data : with_sqlite_database
  {
    static ICollection<NorthwindEmployee> employees;

    Because of = () =>
      employees = session.CreateQuery("FROM NorthwindEmployee").List<NorthwindEmployee>();

    It should_return_objects = () =>
      employees.ShouldNotBeEmpty();
  }
}
