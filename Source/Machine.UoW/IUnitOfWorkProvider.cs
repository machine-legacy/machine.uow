using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkProvider
  {
    IUnitOfWork Start(IUnitOfWorkScope scope, params IUnitOfWorkSettings[] settings);
    IUnitOfWork GetUnitOfWork();
  }
}
