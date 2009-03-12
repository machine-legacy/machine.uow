using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkProvider
  {
    IUnitOfWork Start(params IUnitOfWorkSettings[] settings);
    IUnitOfWork GetUnitOfWork();
  }
}
