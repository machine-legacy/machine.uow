using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machine.UoW.DatabaseContext
{
  public class NoStoredValueException : Exception
  {
    public NoStoredValueException()
    {
    }

    public NoStoredValueException(string message) : base(message)
    {
    }

    public NoStoredValueException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected NoStoredValueException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
