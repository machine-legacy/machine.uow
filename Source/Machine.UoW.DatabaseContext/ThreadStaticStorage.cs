using System;
using System.Collections.Generic;

namespace Machine.UoW.DatabaseContext
{
  public class ThreadStaticStorage<T> : StorageBase<T> where T : class
  {
    [ThreadStatic]
    static Stack<T> _value;

    protected override Stack<T> InternalStack
    {
      get { return _value; }
      set { _value = value; }
    }
  }
}