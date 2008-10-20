using System.Collections.Generic;
using NHibernate;
using Machine.Specifications;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_sqlite_database
  {
    protected static DatabaseViaNHibernate database;

    Establish context = () =>
    {
      database = new DatabaseViaNHibernate();
    };
  }

  [Subject("Querying")]
  public class when_querying_for_data : with_sqlite_database
  {
    static ICollection<NorthwindEmployee> employees;

    Because of = () =>
    {
      ISession session = database.SessionFactory.OpenSession();
      IQuery query = session.CreateQuery("FROM NorthwindEmployee");
      employees = query.List<NorthwindEmployee>();
    };

    It should_return_objects = () => employees.ShouldNotBeEmpty();
  }
}
