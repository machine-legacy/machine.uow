using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.NHibernate.Specs
{
  public class NorthwindDatabase
  {
    readonly IDbConnection _connection;
    IDbTransaction _transaction;

    public NorthwindDatabase(IDbConnection connection)
    {
      _connection = connection;
    }

    public virtual void Create()
    {
      foreach (string create in SqlHelper.Provider.CreateTablesSql())
      {
        SqlHelper.ExecuteNonQuery(_connection, _transaction, create);
      }
    }

    public virtual void DropAllTables()
    {
      foreach (string table in SqlHelper.Provider.TableNames())
      {
        try
        {
          SqlHelper.ExecuteNonQuery(_connection, null, "DROP TABLE " + table);
        }
        catch
        {
        }
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
      return SqlHelper.Provider.ToDateTimeString(dateValue);
    }

    public virtual long AddEntity(string pre, params object[] parameters)
    {
      string sql = String.Format(pre, parameters);
      SqlHelper.ExecuteNonQuery(_connection, _transaction, sql);
      return SqlHelper.Provider.QueryLastRowId(_connection, _transaction);
    }

    public virtual void AddDefaultData()
    {
      _transaction = _connection.BeginTransaction();
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
      _transaction.Commit();
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
