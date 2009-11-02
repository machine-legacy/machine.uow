using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using NHibernate;
using NHibernate.Transaction;

namespace Machine.UoW.NHibernate
{
  public class SorryAboutThisHackToGetTransactionsFromNH
  {
    readonly static System.Func<ITransaction, IDbTransaction> _extractAdoTransaction;

    static SorryAboutThisHackToGetTransactionsFromNH()
    {
      FieldInfo field = typeof(AdoTransaction).GetField("trans", BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == null) throw new ArgumentException();
      _extractAdoTransaction = (nh) => {
        return (IDbTransaction)field.GetValue(nh);
      };
    }

    public static IDbTransaction GetAdoNetTransaction(ISession session)
    {
      if (session.Transaction == null)
        return null;
      return _extractAdoTransaction(session.Transaction);
    }
  }
}
