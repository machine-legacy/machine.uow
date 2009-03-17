using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class NullScopeProvider : IUnitOfWorkScopeProvider
  {
    public IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings)
    {
      return NullScope.Null;
    }
  }
}