using System;
using System.Data;

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
  }
}
