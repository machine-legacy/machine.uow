using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IScopeEvents
  {
    void Start(IUnitOfWorkScope scope);
  }
}
