using System;
using System.Data;

namespace Machine.UoW.NHibernate.Specs
{
  public class DatabaseManager
  {
    IDbConnection _connection;

    public IDbConnection Connection
    {
      get { return _connection; }
    }

    public void Recreate()
    {
      _connection = SqlHelper.Provider.CreateConnection();
      _connection.Open();
      NorthwindDatabase database = new NorthwindDatabase(_connection);
      database.DropAllTables();
      database.Create();
    }

    public void Close()
    {
      _connection.Close();
    }
  }
}