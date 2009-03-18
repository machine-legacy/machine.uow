using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

using Machine.UoW.AdoDotNet;
using Machine.UoW.AmbientTransactions;
using Machine.UoW.NHibernate.Specs.NorthwindModel;

using Machine.Specifications;
using Machine.UoW.Specs;
using NHibernate;
using IsolationLevel=System.Data.IsolationLevel;

namespace Machine.UoW.NHibernate.Specs.AmbientTransactions
{
  public static class AdoNetHelpers
  {
    public static object Query(this IDbConnection connection, string sql)
    {
      var cmd = connection.CreateCommand();
      cmd.CommandText = sql;
      return cmd.ExecuteScalar();
    }
  }

  [Subject("Ambient Scope, Saving objects")]
  public class when_saving_an_object : with_nhibernate_uow_and_ambient_scope_provider
  {
    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope())
      {
        using (var transaction = UoW.BeginTransaction(IsolationLevel.Unspecified))
        {
          employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          transaction.Commit();
        }
        scope.Complete();
      }
      using (new TransactionScope())
      {
        employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Ambient Scope, Saving objects")]
  public class when_saving_an_object_inside_distributed_transaction : with_nhibernate_uow_and_ambient_scope_provider
  {
    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
      {
        using (var sql = SqlHelper.Provider.OpenConnection())
        {
          sql.Query("SELECT @@TRANCOUNT");
        }
        using (var transaction = UoW.BeginTransaction(IsolationLevel.Unspecified))
        {
          employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          transaction.Commit();
        }
        scope.Complete();
      }
      using (new TransactionScope())
      {
        employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_be_changed_in_database = () =>
      employee.FirstName.ShouldEqual("Steve Van");
  }

  [Subject("Ambient Scope, Saving objects")]
  public class when_saving_an_object_but_not_completing_scope : with_nhibernate_uow_and_ambient_scope_provider
  {
    Because of = () =>
    {
      using (TransactionScope scope = new TransactionScope())
      {
        using (var transaction = UoW.BeginTransaction(IsolationLevel.Unspecified))
        {
          employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
          employee.FirstName = "Steve Van";
          transaction.Commit();
        }
      }
      using (new TransactionScope())
      {
        employee = UoW.Scope().Session().Get<NorthwindEmployee>(id);
      }
    };

    It should_not_be_changed_in_database = () =>
      employee.FirstName.ShouldNotEqual("Steve Van");
  }

  public class with_nhibernate_uow_and_ambient_scope_provider
  {
    protected static Exception exception;
    protected static NorthwindEmployee employee;
    protected static long id;

    Establish context = () =>
    {
      LoggingStartup loggingStartup = new LoggingStartup();
      loggingStartup.Start();
      ISessionFactory sessionFactory;

      using (var scope = new TransactionScope())
      {
        DatabaseAndNhSessionFactory database = new DatabaseAndNhSessionFactory();
        database.Open();
        sessionFactory = database.SessionFactory;
        id = database.AddSingleEmployee();
        database.Close();
        scope.Complete();
      }

      IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
      unitOfWorkManagement.AddEvents(new AdoNetConnectionScopeEvents(SqlHelper.Provider));
      unitOfWorkManagement.AddEvents(new NHibernateScopeEvents(sessionFactory));
      IUnitOfWorkFactory factory = new UnitOfWorkFactory(unitOfWorkManagement);
      IUnitOfWorkScopeProvider scopeProvider = new AmbientTransactionUnitOfWorkScopeProvider(factory);
      UoW.Startup(new NullUnitOfWorkProvider(scopeProvider), scopeProvider, new NHibernateTransactionProvider(scopeProvider));
    };
  }
}