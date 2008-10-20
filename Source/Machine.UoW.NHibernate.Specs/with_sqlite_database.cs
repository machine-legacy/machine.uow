using System;
using System.Collections.Generic;

using NHibernate;

using Machine.Specifications;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_sqlite_database
  {
    protected static DatabaseViaNHibernate database;
    protected static ISession session;

    Establish context = () =>
    {
      database = new DatabaseViaNHibernate();
      database.Open();
      session = database.SessionFactory.OpenSession();
    };

    Cleanup after = () =>
    {
      database.Close();
    };
  }
}
