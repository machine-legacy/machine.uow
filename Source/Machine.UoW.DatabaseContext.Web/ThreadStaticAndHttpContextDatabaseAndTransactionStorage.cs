using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class ThreadStaticAndHttpContextStorage<T> : IContextStorage<T> where T : class
  {
    IContextStorage<T> CurrentStorage
    {
      get
      {
        if (HttpContext.Current == null)
          return _threadStaticContextContextStorage;
        return _httpContextContextStorage;
      }
    }

    readonly IContextStorage<T> _threadStaticContextContextStorage = new ThreadStaticStorage<T>();
    readonly IContextStorage<T> _httpContextContextStorage = new HttpContextStorage<T>();

    public T Peek()
    {
      return CurrentStorage.Peek();
    }

    public void Push(T value)
    {
      CurrentStorage.Push(value);
    }

    public bool IsEmpty
    {
      get { return CurrentStorage.IsEmpty; }
    }

    public T Pop()
    {
      return CurrentStorage.Pop();
    }
  }
}