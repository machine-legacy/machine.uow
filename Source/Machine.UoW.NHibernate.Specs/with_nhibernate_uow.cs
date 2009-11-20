using System;
using System.Collections.Generic;

using Machine.Specifications;
using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public class with_nhibernate_uow : with_sqlite_database
  {
    protected static Exception exception;

    Establish context = () =>
    {
      SpecDatabase.Startup(new TransientSessionManager(database.SessionFactory), new TransientConnectionManager(SqlHelper.Provider));
    };
  }
}