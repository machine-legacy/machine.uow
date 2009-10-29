using System;
using System.Collections.Generic;

namespace Machine.UoW.ContextScope
{
  public interface ISession
  {
    void Complete();
  }
}
