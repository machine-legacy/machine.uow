using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkScopeProvider
  {
    IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings);
  }
}