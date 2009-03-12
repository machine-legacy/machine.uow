using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class UnitOfWorkExtensions
  {
    public static ISession Session(this IUnitOfWork scope)
    {
      if (scope == null)
      {
        throw new InvalidOperationException("No current UoW");
      }
      return scope.Scope.Get<CurrentSession>().Session;
    }
  }
}
