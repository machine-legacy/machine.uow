using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class HttpContextStorage<T> : StorageBase<T> where T : class
  {
    protected override T InternalValue
    {
      get { return (T)HttpContext.Current.Items[typeof(HttpContextStorage<T>).FullName]; }
      set { HttpContext.Current.Items[typeof(HttpContextStorage<T>).FullName] = value; }
    }
  }
}