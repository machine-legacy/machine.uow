using System;

namespace Machine.UoW
{
  public interface IUnitOfWorkFactory
  {
    IUnitOfWork StartUnitOfWork();
  }
}