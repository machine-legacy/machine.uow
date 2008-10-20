using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.UoW.NHibernate.Specs
{
  public static class SqliteHelper
  {
    public static string[] GetTableNames(IDbConnection connection)
    {
      string sql = "SELECT name FROM sqlite_master WHERE type = 'table';";
      IDataReader reader = Execute(connection, sql);
      return GetStringArray(reader, 0);
    }

    public static string[] GetColumnNames(IDbConnection connection, string table)
    {
      string sql = String.Format("PRAGMA table_info({0});", table);
      IDataReader reader = Execute(connection, sql);
      return GetStringArray(reader, 1);
    }

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

    public static void ExecuteNonQuery(IDbConnection connection, string sql, params object[] args)
    {
      IDbCommand command = connection.CreateCommand();
      command.CommandText = String.Format(sql, args);
      command.ExecuteNonQuery();
    }

    public static long QueryLastInsertRowId(IDbConnection connection)
    {
      IDbCommand command = connection.CreateCommand();
      command.CommandText = "SELECT last_insert_rowid()";
      return (long)command.ExecuteScalar();
    }
  }
}
