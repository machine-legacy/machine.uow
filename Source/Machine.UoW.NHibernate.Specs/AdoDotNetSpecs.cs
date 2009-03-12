using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

using Machine.Specifications;
using Machine.UoW.AdoDotNet;
using Machine.UoW.AmbientTransactions;
using Machine.UoW.NHibernate.Specs;
using Machine.UoW.Specs;

namespace Machine.UoW.NHibernate.AdoNetSpecs
{
  public class AdoDotNetSpecs
  {
    protected static IUnitOfWorkFactory factory;
    protected static IDbConnection connection;
    protected static Exception exception;
    protected static IDbConnection first;
    protected static IDbConnection second;

    Establish context = () =>
    {
      LoggingStartup loggingStartup = new LoggingStartup();
      loggingStartup.Start();
      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new AdoNetConnectionScopeEvents(SqlHelper.Provider));
      factory = new UnitOfWorkFactory(unitOfWorkManagement);
      UoW.Provider = new HybridUnitOfWorkProvider(factory);
      UoW.ScopeProvider = new ThreadStaticUnitOfWorkScopeProvider(factory);
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
      using (UoW.Scope)
      {
        connection = UoW.Scope.Connection();
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
      using (UoW.Scope)
      {
        first = UoW.Scope.Connection();
        second = UoW.Scope.Connection();
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
      using (UoW.Scope)
      {
        first = UoW.Scope.Connection();
      }
      using (UoW.Scope)
      {
        second = UoW.Scope.Connection();
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
      UoW.ScopeProvider = new AmbientTransactionUnitOfWorkScopeProvider(factory);
    };
    
    Because of = () =>
    {
      using (new TransactionScope())
      {
        first = UoW.Scope.Connection();
      }
      exception = Catch.Exception(() => UoW.Scope.Connection());
      using (new TransactionScope())
      {
        second = UoW.Scope.Connection();
      }
    };

    It should_throw_when_used_outside_of_scope = () =>
      exception.ShouldNotBeNull();

    It should_not_be_null = () =>
      second.ShouldNotBeNull();

    It should_not_be_equal = () =>
      second.ShouldNotEqual(first);
  }
}
