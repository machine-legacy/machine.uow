using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface ITransaction : IDisposable
  {
    void Commit();
    void Rollback();
  }

  public class NullTransaction : ITransaction
  {
    public void Dispose()
    {
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }
  }
}
