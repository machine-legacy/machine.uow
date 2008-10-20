using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class UnitOfWorkExtensions
  {
    public static ISession Session(this IUnitOfWorkState state)
    {
      return state.Get<CurrentSession>().Session;
    }
  }
}
