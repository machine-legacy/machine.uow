using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW.AmbientTransactions
{
  [Flags]    
  public enum AmbientTransactionOption
  {
    None = 0,
    UseOuterScope = 1,
    AlwaysSuppress = 2
  }

  public class AmbientTransactionSettings : IUnitOfWorkSettings
  {
    public static AmbientTransactionSettings Default = new AmbientTransactionSettings(AmbientTransactionOption.None);
    readonly AmbientTransactionOption _flags;

    private bool ShouldUseOuterScope
    {
      get { return (_flags & AmbientTransactionOption.UseOuterScope) == AmbientTransactionOption.UseOuterScope; }
    }

    private bool AlwaysSuppress
    {
      get { return (_flags & AmbientTransactionOption.AlwaysSuppress) == AmbientTransactionOption.AlwaysSuppress; }
    }

    private TransactionScopeOption ToOption()
    {
      if (this.AlwaysSuppress)
      {
        return TransactionScopeOption.Suppress;
      }
      return TransactionScopeOption.Required;
    }

    public TransactionScope ToScope()
    {
      return new TransactionScope(ToOption());
    }

    protected AmbientTransactionSettings(AmbientTransactionOption flags)
    {
      _flags = flags;
    }

    public static AmbientTransactionSettings Create(AmbientTransactionOption flags)
    {
      return new AmbientTransactionSettings(flags);
    }
  }

  public static class AmbientSettingsHelpers
  {
    public static AmbientTransactionSettings AmbientSettings(this IUnitOfWorkSettings[] settings)
    {
      foreach (IUnitOfWorkSettings setting in settings)
      {
        if (typeof(AmbientTransactionSettings).IsInstanceOfType(setting))
        {
          return (AmbientTransactionSettings)setting;
        }
      }
      return AmbientTransactionSettings.Default;
    }
  }
}
