using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

using Machine.UoW.AdoDotNet;

namespace Machine.UoW.NHibernate.Specs
{
  public interface IDatabaseProvider : IConnectionProvider
  {
    long QueryLastRowId(IDbConnection connection, IDbTransaction transaction);
    string[] CreateTablesSql();
    string[] TableNames();
    string ToDateTimeString(DateTime dateValue);
    IDictionary<string, string> CreateNhibernateProperties();
  }

  public class SqliteProvider : IDatabaseProvider
  {
    private string _filename = "Northwind.db";

    public long QueryLastRowId(IDbConnection connection, IDbTransaction transaction)
    {
      IDbCommand command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = "SELECT last_insert_rowid()";
      return (long) command.ExecuteScalar();
    }

    public string[] CreateTablesSql()
    {
      return new[] {
        @"CREATE TABLE Employees (
  		    EmployeeId INTEGER PRIMARY KEY,
		      FirstName VARCHAR(100),
		      LastName VARCHAR(100),
		      BirthDate DATETIME,
		      IsAlive BOOL,
		      ReportsTo INTEGER REFERENCES Employees
  		  );",
        @"CREATE TABLE Categories (
		      CategoryId INTEGER PRIMARY KEY,
		      CategoryName VARCHAR(100)
  		  );",
        @"CREATE TABLE Products (
		      ProductId INTEGER PRIMARY KEY,
		      ProductName VARCHAR(100),
		      CategoryId INTEGER REFERENCES Categories
  		  );",
        @"CREATE TABLE Customers (
		      CustomerId INTEGER PRIMARY KEY,
		      CustomerName VARCHAR(100)
  		  );",
        @"CREATE TABLE Orders (
		      OrderId INTEGER PRIMARY KEY,
          Title VARCHAR(100),
		      CustomerId INTEGER REFERENCES Customers
  		  );",
        @"CREATE TABLE OrderLines (
		      OrderLineId INTEGER PRIMARY KEY,
          Title VARCHAR(100),
		      OrderId INTEGER REFERENCES Orders
  		  );"
      };
    }

    public string[] TableNames()
    {
      return new[] { "OrderLines", "Orders", "Customers", "Products", "Categories", "Employees" };
    }

    public string ToDateTimeString(DateTime dateValue)
    {
      return dateValue.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
    }

    public IDictionary<string, string> CreateNhibernateProperties()
    {
      Dictionary<string, string> properties = new Dictionary<string, string>();
      properties["connection.driver_class"] = "NHibernate.Driver.SQLite20Driver";
      properties["dialect"] = "NHibernate.Dialect.SQLiteDialect";
      return properties;
    }

    public IDbConnection OpenConnection()
    {
      DeleteIfExists();
      bool recreateNecessary = true;
      string connectionString = String.Format("Data Source={0};Version=3;New={1};Compress=False", _filename, recreateNecessary ? "True" : "False");
      IDbConnection connection = new SQLiteConnection(connectionString);
      connection.Open();
      return connection;
    }

    private void DeleteIfExists()
    {
      if (File.Exists(_filename))
      {
        File.Delete(_filename);
      }
    }
  }

  public class SqlServerProvider : IDatabaseProvider
  {
    public long QueryLastRowId(IDbConnection connection, IDbTransaction transaction)
    {
      IDbCommand command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = "SELECT @@IDENTITY";
      return (long) ((decimal) command.ExecuteScalar());
    }

    public string[] CreateTablesSql()
    {
      return new[] {
        @"CREATE TABLE Employees (
    		EmployeeId INTEGER IDENTITY(1, 1) PRIMARY KEY,
      FirstName VARCHAR(100),
		      LastName VARCHAR(100),
		      BirthDate DATETIME,
		      IsAlive BIT,
		      ReportsTo INTEGER REFERENCES Employees
      )",
      @"CREATE TABLE Categories (
		      CategoryId INTEGER IDENTITY(1, 1) PRIMARY KEY,
		      CategoryName VARCHAR(100)
		  );",
      @"CREATE TABLE Products (
		      ProductId INTEGER IDENTITY(1, 1) PRIMARY KEY,
		      ProductName VARCHAR(100),
		      CategoryId INTEGER REFERENCES Categories
		  );",
      @"CREATE TABLE Customers (
		      CustomerId INTEGER IDENTITY(1, 1) PRIMARY KEY,
		      CustomerName VARCHAR(100)
		  );",
      @"CREATE TABLE Orders (
		      OrderId INTEGER IDENTITY(1, 1) PRIMARY KEY,
          Title VARCHAR(100),
		      CustomerId INTEGER REFERENCES Customers
		  );",
      @"CREATE TABLE OrderLines (
		      OrderLineId INTEGER IDENTITY(1, 1) PRIMARY KEY,
          Title VARCHAR(100),
		      OrderId INTEGER REFERENCES Orders
		  );"
      };
    }

    public string[] TableNames()
    {
      return new[] { "OrderLines", "Orders", "Customers", "Products", "Categories", "Employees" };
    }

    public string ToDateTimeString(DateTime dateValue)
    {
      return dateValue.ToString();
    }

    public IDictionary<string, string> CreateNhibernateProperties()
    {
      Dictionary<string, string> properties = new Dictionary<string, string>();
      properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver, NHibernate";
      properties["dialect"] = "NHibernate.Dialect.MsSql2005Dialect, NHibernate";
      return properties;
    }

    public IDbConnection OpenConnection()
    {
      IDbConnection connection = new SqlConnection("Server=127.0.0.1;Initial Catalog=northwind;Integrated Security=true;Pooling=False");
      connection.Open();
      return connection;
    }
  }

  public static class SqlHelper
  {
    public static IDatabaseProvider Provider = new SqlServerProvider();

    public static IDataReader Execute(IDbConnection connection, string sql)
    {
      IDbCommand command = connection.CreateCommand();
      command.CommandText = sql;
      return command.ExecuteReader();
    }

    public static string[] GetStringArray(IDataReader reader, int index)
    {
      List<string> values = new List<string>();
      while (reader.Read())
      {
        values.Add(reader.GetString(index));
      }
      reader.Close();
      return values.ToArray();
    }

    public static void ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, string sql, params object[] args)
    {
      IDbCommand command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = String.Format(sql, args);
      command.ExecuteNonQuery();
    }
  }
}