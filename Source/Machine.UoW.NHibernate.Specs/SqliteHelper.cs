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
      IDataReader reader = SqlHelper.Execute(connection, sql);
      return SqlHelper.GetStringArray(reader, 0);
    }

    public static string[] GetColumnNames(IDbConnection connection, string table)
    {
      string sql = String.Format("PRAGMA table_info({0});", table);
      IDataReader reader = SqlHelper.Execute(connection, sql);
      return SqlHelper.GetStringArray(reader, 1);
    }
  }
}
