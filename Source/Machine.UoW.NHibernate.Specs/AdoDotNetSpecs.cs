using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

using Machine.Specifications;
using Machine.UoW.AdoDotNet;
using Machine.UoW.DatabaseContext;
using Machine.UoW.NHibernate.Specs;
using Machine.UoW.Specs;

namespace Machine.UoW.NHibernate.AdoNetSpecs
{
  public class AdoDotNetSpecs
  {
    protected static IDbConnection connection;
    protected static Exception exception;
    protected static IDbConnection first;
    protected static IDbConnection second;

    Establish context = () =>
    {
      LoggingStartup loggingStartup = new LoggingStartup();
      loggingStartup.Start();
      SpecDatabase.Startup(new NullSessionManager(), new TransientConnectionManager(SqlHelper.Provider));
      first = null;
      second = null;
      connection = null;
    };
  }

  [Subject("ADO.NET")]
  public class when_first_retrieving_connection : AdoDotNetSpecs
  {
    Because of = () =>
    {
      using (SpecDatabase.OpenConnection())
      {
        connection = Database.Connection;
      }
    };

    It should_not_be_null = () =>
      connection.ShouldNotBeNull();
  }

  [Subject("ADO.NET")]
  public class when_retrieving_connection_twice : AdoDotNetSpecs
  {
    Because of = () =>
    {
      using (SpecDatabase.OpenConnection())
      {
        first = Database.Connection;
        second = Database.Connection;
      }
    };

    It should_not_be_null = () =>
      second.ShouldNotBeNull();

    It should_be_equal = () =>
      second.ShouldEqual(first);
  }

  [Subject("ADO.NET")]
  public class when_retrieving_connection_twice_in_separate_scopes : AdoDotNetSpecs
  {
    Because of = () =>
    {
      using (SpecDatabase.OpenConnection())
      {
        first = Database.Connection;
      }
      using (SpecDatabase.OpenConnection())
      {
        second = Database.Connection;
      }
    };

    It should_not_be_null = () =>
      second.ShouldNotBeNull();

    It should_not_be_equal = () =>
      second.ShouldNotEqual(first);
  }

  [Subject("ADO.NET")]
  public class when_retrieving_connection_twice_inside_a_transaction_scope : AdoDotNetSpecs
  {
    Establish context = () =>
    {
      SpecDatabase.Startup(new NullSessionManager(), new TransientConnectionManager(SqlHelper.Provider));
    };
    
    Because of = () =>
    {
      using (new TransactionScope())
      using (SpecDatabase.OpenConnection())
      {
        first = Database.Connection;
      }
      using (new TransactionScope())
      using (SpecDatabase.OpenConnection())
      {
        second = Database.Connection;
      }
    };

    It should_not_be_null = () =>
      second.ShouldNotBeNull();

    It should_not_be_equal = () =>
      second.ShouldNotEqual(first);
  }
}
