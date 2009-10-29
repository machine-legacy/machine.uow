using System;
using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public class ThreadStaticStorage<T> : StorageBase<T> where T : class
  {
    [ThreadStatic]
    static T _value;

    protected override T InternalValue
    {
      get { return _value; }
      set { _value = value; }
    }
  }
}