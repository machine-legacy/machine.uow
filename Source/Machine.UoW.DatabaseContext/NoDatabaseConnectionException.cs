using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machine.UoW.DatabaseContext
{
  public class NoDatabaseConnectionException : Exception
  {
    public NoDatabaseConnectionException()
    {
    }

    public NoDatabaseConnectionException(string message) : base(message)
    {
    }

    public NoDatabaseConnectionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected NoDatabaseConnectionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
