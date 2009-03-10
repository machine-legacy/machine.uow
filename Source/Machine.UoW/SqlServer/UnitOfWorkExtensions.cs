using System;
using System.Data;
using System.Data.SqlClient;

namespace Machine.UoW.SqlServer
{
  public static class UnitOfWorkExtensions
  {
    public static IDbConnection Connection(this IUnitOfWorkState state)
    {
      if (state == null)
      {
        throw new InvalidOperationException("No current UoW");
      }
      return state.Get<CurrentConnection>().Connection();
    }

    public static SqlConnection Sql(this IUnitOfWorkState state)
    {
      return (SqlConnection)state.Connection();
    }
  }
}
