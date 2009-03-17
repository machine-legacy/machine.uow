using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface ITransaction : IDisposable
  {
    void Commit();
    void Rollback();
  }
}
