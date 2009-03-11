using System.Collections.Generic;

namespace Machine.UoW
{
  public interface IUnitOfWorkScope
  {
    T Get<T>(T defaultValue);
    T Get<T>();
    void Set<T>(T value);
  }
}