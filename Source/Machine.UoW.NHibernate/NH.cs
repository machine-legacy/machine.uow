using System;
using System.Collections.Generic;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public static class NH
  {
    [ThreadStatic]
    static ISession _session;

    public static ISession Session
    {
      get
      {
        return _session;
      }
      set
      {
        if (_session != value)
        {
          if (_session != null && value != null)
          {
            throw new InvalidOperationException("Trying to use another Session when one is already in use!");
          }
        }
        _session = value;
      }
    }
  }
}