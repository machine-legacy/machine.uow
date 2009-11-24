using System;
using System.Data;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class HttpContextStorage<T> : StorageBase<T> where T : class
  {
    readonly Guid _id = Guid.NewGuid();

    protected override T InternalValue
    {
      get { return (T) HttpContext.Current.Items[_id.ToString()]; }
      set { HttpContext.Current.Items[_id.ToString()] = value; }
    }
  }
}