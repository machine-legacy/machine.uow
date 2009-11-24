using System;
using System.Collections.Generic;
using System.Web;

namespace Machine.UoW.DatabaseContext.Web
{
  public class HttpContextStorage<T> : StorageBase<T> where T : class
  {
    readonly Guid _id = Guid.NewGuid();

    protected override Stack<T> InternalStack
    {
      get { return (Stack<T>)HttpContext.Current.Items[_id.ToString()]; }
      set { HttpContext.Current.Items[_id.ToString()] = value; }
    }
  }
}