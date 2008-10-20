using System;
using System.Data.SQLite;
using System.IO;

namespace Machine.UoW.NHibernate.Specs
{
  public class SqliteDatabase
  {
    private static string _filename = "Northwind.db";
    private static bool _firstTime = true;
    private static SQLiteConnection _connection;
    private static NorthwindDatabase _database;

    public SQLiteConnection Connection
    {
      get { return _connection; }
    }

    public virtual void Open()
    {
      if (_connection != null)
      {
        SqliteHelper.ExecuteNonQuery(_connection, "ROLLBACK");
        _connection.Close();
        _connection = null;
      }

      if (_connection == null)
      {
        bool createNecessary = !File.Exists(_filename) || _firstTime;
        if (createNecessary && File.Exists(_filename))
        {
          File.Delete(_filename);
        }
        string connectionString = String.Format("Data Source={0};Version=3;New={1};Compress=False;", _filename, createNecessary ? "True" : "False");
        _connection = new SQLiteConnection(connectionString);
        _connection.Open();
        if (createNecessary)
        {
          CreateDatabase(_connection);
        }
        _firstTime = false;
      }
      SqliteHelper.ExecuteNonQuery(_connection, "BEGIN");
    }

    public virtual void Close()
    {
      SqliteHelper.ExecuteNonQuery(_connection, "ROLLBACK");
      _connection.Close();
      _connection = null;
    }

    protected virtual void CreateDatabase(SQLiteConnection connection)
    {
      _database = new NorthwindDatabase(connection);
      _database.DropAllTables();
      _database.Create();
    }
  }
}