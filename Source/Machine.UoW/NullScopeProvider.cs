using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class NullScopeProvider : IUnitOfWorkScopeProvider
  {
    public IUnitOfWorkScope GetUnitOfWorkScope()
    {
      return NullScope.Null;
    }
  }
}