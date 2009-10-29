using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class ThreadStaticAndHttpContextStorage<T> : IContextStorage<T> where T : class
  {
    public T StoredValue
    {
      get { return this.CurrentStorage.StoredValue; }
      set { this.CurrentStorage.StoredValue = value; }
    }

    public bool HasValue
    {
      get { return this.CurrentStorage.HasValue; }
    }

    IContextStorage<T> CurrentStorage
    {
      get
      {
        if (HttpContext.Current == null)
          return _threadStaticAndHttpContextContextStorage;
        return _httpContextContextStorage;
      }
    }

    readonly IContextStorage<T> _threadStaticAndHttpContextContextStorage = new ThreadStaticStorage<T>();
    readonly IContextStorage<T> _httpContextContextStorage = new HttpContextStorage<T>();
  }
}