using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Machine.UoW.NHibernate.Specs
{
  public class NorthwindDatabase
  {
    private readonly IDbConnection _connection;

    public NorthwindDatabase(IDbConnection connection)
    {
      _connection = connection;
    }

    public virtual void Create()
    {
      string[] sql = new string[] {
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
      foreach (string create in sql)
      {
        SqliteHelper.ExecuteNonQuery(_connection, create);
      }
      AddDefaultData();
    }

    public virtual void DropAllTables()
    {
      foreach (string table in SqliteHelper.GetTableNames(_connection))
      {
        SqliteHelper.ExecuteNonQuery(_connection, "DROP TABLE " + table);
      }
    }

    public virtual long AddEmployee(string firstName, string lastName, DateTime birthday, bool isAlive, long reportsTo)
    {
      return AddEntity("INSERT INTO Employees (FirstName, LastName, BirthDate, IsAlive, ReportsTo) VALUES ('{0}', '{1}', '{2}', {3}, {4})", firstName, lastName, ToString(DateTime.Now), isAlive ? 1 : 0, reportsTo > 0 ? reportsTo.ToString() : "NULL");
    }

    public virtual long AddCustomer(string name)
    {
      return AddEntity("INSERT INTO Customers (CustomerName) VALUES ('{0}')", name);
    }

    public virtual long AddOrder(long customer, string title)
    {
      return AddEntity("INSERT INTO Orders (CustomerId, Title) VALUES ('{0}', '{1}')", customer, title);
    }

    public virtual long AddOrderLine(long order, string title)
    {
      return AddEntity("INSERT INTO OrderLines (OrderId, Title) VALUES ('{0}', '{1}')", order, title);
    }

    public virtual long AddCategory(string name)
    {
      return AddEntity("INSERT INTO Categories (CategoryName) VALUES ('{0}')", name);
    }

    public virtual long AddProduct(string name, long category)
    {
      return AddEntity("INSERT INTO Products (ProductName, CategoryId) VALUES ('{0}', {1})", name, category);
    }

    public virtual string ToString(DateTime dateValue)
    {
      return dateValue.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture); 
    }

    public virtual long AddEntity(string pre, params object[] parameters)
    {
      string sql = String.Format(pre, parameters);
      SqliteHelper.ExecuteNonQuery(_connection, sql);
      return SqliteHelper.QueryLastInsertRowId(_connection);
    }

    public virtual void AddDefaultData()
    {
      IDbTransaction transaction = _connection.BeginTransaction();
      long superBoss = AddEmployee("Craig", "Boucher", DateTime.Today, true, 0);
      long boss = AddEmployee("Jacob", "Lewallen", DateTime.Today, true, superBoss);
      AddEmployee("Bruce", "Springsteen", DateTime.Today, true, 0);
      AddEmployee("John", "McClane", DateTime.Today, true, 0);
      AddEmployee("Andrea", "Lewallen", DateTime.Today, true, boss);
      AddEmployee("Aaron", "Jensen", DateTime.Today, true, boss);

      long electronics = AddCategory("Electronics");
      AddProduct("iPod", electronics);
      AddProduct("Nintendo DS", electronics);
      AddProduct("Camera", electronics);

      long sports = AddCategory("Sports");
      AddProduct("Soccer", sports);
      AddProduct("Football", sports);

      AddCustomerData("Cyberdyne", 2);
      AddCustomerData("ACME", 4);
      transaction.Commit();
    }

    public virtual void AddCustomerData(string customer, int orders)
    {
      long id = AddCustomer(customer);
      for (int i = 0; i < orders; i++)
      {
        long order = AddOrder(id, String.Format("{0} #{1}", customer, i));
        for (int j = 0; j < 2; j++)
        {
          AddOrderLine(order, String.Format("{0} #{1} Line #{2}", customer, i, j));
        }
      }
    }
  }
}
