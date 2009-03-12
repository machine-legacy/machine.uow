using System;
using System.Data;
using System.Data.SqlClient;

namespace Machine.UoW.AdoDotNet
{
  public static class UnitOfWorkExtensions
  {
    public static IDbConnection Connection(this IUnitOfWork scope)
    {
      if (scope == null)
      {
        throw new InvalidOperationException("No current UoW");
      }
      return scope.Scope.Get<CurrentConnection>().Connection();
    }

    public static SqlConnection Sql(this IUnitOfWork scope)
    {
      return (SqlConnection)scope.Connection();
    }
  }
}
