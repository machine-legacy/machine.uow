using System;
using System.Data;
using System.Data.SqlClient;

namespace Machine.UoW.AdoDotNet
{
  public static class UnitOfWorkExtensions
  {
    public static IDbConnection Connection(this IUnitOfWork scope)
    {
      if (scope == null) throw new ArgumentException("scope");
      return scope.Scope.Connection();
    }

    public static SqlConnection Sql(this IUnitOfWork scope)
    {
      if (scope == null) throw new ArgumentException("scope");
      return scope.Scope.Sql();
    }
    
    public static IDbConnection Connection(this IUnitOfWorkScope scope)
    {
      if (scope == null) throw new ArgumentException("scope");
      return scope.Get<CurrentConnection>().Connection();
    }

    public static SqlConnection Sql(this IUnitOfWorkScope scope)
    {
      return (SqlConnection)scope.Connection();
    }
  }
}
