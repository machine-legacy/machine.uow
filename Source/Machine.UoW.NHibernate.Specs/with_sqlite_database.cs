using System;
using System.Collections.Generic;

using NHibernate;

using Machine.Specifications;

namespace Machine.UoW.NHibernate.Specs
{
  public abstract class with_sqlite_database
  {
    protected static DatabaseAndNhSessionFactory database;
    protected static ISession session;

    Establish context = () =>
    {
      database = new DatabaseAndNhSessionFactory();
      database.Open();
      session = database.SessionFactory.OpenSession();
    };

    Cleanup after = () =>
    {
      database.Close();
    };
  }
}
