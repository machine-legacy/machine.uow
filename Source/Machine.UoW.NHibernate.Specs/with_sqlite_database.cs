using System;
using System.Collections.Generic;

using NHibernate;

using Machine.Specifications;
using Machine.UoW.Specs;

namespace Machine.UoW.NHibernate.Specs
{
  public abstract class with_sqlite_database
  {
    protected static DatabaseAndNhSessionFactory database;

    Establish context = () =>
    {
      LoggingStartup loggingStartup = new LoggingStartup();
      loggingStartup.Start();
      database = new DatabaseAndNhSessionFactory();
      database.Open();
    };

    Cleanup after = () =>
    {
      database.Close();
    };
  }
}
