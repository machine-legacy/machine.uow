using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machine.UoW.AdoDotNet
{
  public class NoDatabaseTransactionException : Exception
  {
    public NoDatabaseTransactionException()
    {
    }

    public NoDatabaseTransactionException(string message) : base(message)
    {
    }

    public NoDatabaseTransactionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected NoDatabaseTransactionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
