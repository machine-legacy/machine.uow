using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class UnitOfWorkExtensions
  {
    public static ISession Session(this IUnitOfWorkState state)
    {
      if (state == null)
      {
        throw new InvalidOperationException("No current UoW");
      }
      return state.Get<CurrentSession>().Session;
    }
  }
}
