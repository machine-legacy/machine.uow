using System;
using System.Collections.Generic;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class UnitOfWorkExtensions
  {
    public static ISession Session(this IUnitOfWork scope)
    {
      if (scope == null) throw new ArgumentException("scope");
      return scope.Scope.Session();
    }
    
    public static ISession Session(this IUnitOfWorkScope scope)
    {
      if (scope == null) throw new ArgumentException("scope");
      return scope.Get<CurrentSession>().Session;
    }
  }
}
