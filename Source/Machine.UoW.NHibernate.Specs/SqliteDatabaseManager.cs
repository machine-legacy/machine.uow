using System;
using System.Data.SQLite;
using System.IO;

namespace Machine.UoW.NHibernate.Specs
{
  public class SqliteDatabaseManager
  {
    private string _filename = "Northwind.db";
    private SQLiteConnection _connection;

    public SQLiteConnection Connection
    {
      get { return _connection; }
    }

    public void Recreate()
    {
      DeleteIfExists();
      CreateOrOpen();
    }

    public void CreateOrOpen()
    {
      bool recreateNecessary = !Exists();
      string connectionString = String.Format("Data Source={0};Version=3;New={1};Compress=False;", _filename, recreateNecessary ? "True" : "False");
      _connection = new SQLiteConnection(connectionString);
      _connection.Open();
      if (recreateNecessary)
      {
        RecreateDatabase();
      }
    }

    private void RecreateDatabase()
    {
      NorthwindDatabase database = new NorthwindDatabase(_connection);
      database.DropAllTables();
      database.Create();
    }

    private void DeleteIfExists()
    {
      if (Exists())
      {
        File.Delete(_filename);
      }
    }

    private bool Exists()
    {
      return File.Exists(_filename);
    }

    public void Close()
    {
      _connection.Close();
    }
  }
}