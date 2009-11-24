using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.UoW.DatabaseContext
{
  public abstract class StorageBase<T> : IContextStorage<T> where T : class
  {
    protected abstract Stack<T> InternalStack { get; set; }
    protected Stack<T> Stack
    {
      get
      {
        var stack = this.InternalStack ?? new Stack<T>();
        this.InternalStack = stack;
        return stack;
      }
    }

    public T Peek()
    {
      if (IsEmpty)
      {
        throw new NoStoredValueException("There is no " + typeof(T) + " available!");
      }
      return Stack.Peek();
    }

    public void Push(T value)
    {
      Stack.Push(value);
    }

    public bool IsEmpty
    {
      get { return !Stack.Any(); }
    }

    public T Pop()
    {
      return Stack.Pop();
    }
  }
}