using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW.AmbientTransactions
{
  public static class AmbientTransactionHelpers
  {
    public static bool InAmbientTransaction()
    {
      return Transaction.Current != null;
    }
  }
}
