using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
        if (_session == null)
        {
          throw new NoNHibernateSessionException();
        }
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

  public class NoNHibernateSessionException : Exception
  {
    public NoNHibernateSessionException()
    {
    }

    public NoNHibernateSessionException(string message) : base(message)
    {
    }

    public NoNHibernateSessionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected NoNHibernateSessionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}