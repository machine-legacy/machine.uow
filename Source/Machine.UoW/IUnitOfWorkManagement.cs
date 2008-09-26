using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkManagement : IUnitOfWork
  {
    IEnumerable<UnitOfWorkEntry> Entries
    {
      get;
    }

    IEnumerable<UnitOfWorkChange> Changes
    {
      get;
    }
  }
}