using System;
using System.Runtime.Serialization;

namespace Machine.UoW.NHibernate
{
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