using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public class UnitOfWorkFactory : IUnitOfWorkFactory
  {
    public IUnitOfWork StartUnitOfWork()
    {
      return new UnitOfWork();
    }
  }
}
