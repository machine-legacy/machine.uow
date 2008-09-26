using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWork
  {
    void Save<T>(T instance);
    void Delete<T>(T instance);
  }
}
