using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public static class AmbientTransactionHelpers
  {
    public static bool InAmbientTransaction()
    {
      return System.Transactions.Transaction.Current != null;
    }
  }
}
