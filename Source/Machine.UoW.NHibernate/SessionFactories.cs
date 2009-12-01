using System;
using System.Collections.Generic;
using Machine.Core.Utility;
using NHibernate;
using NHibernate.Context;

namespace Machine.UoW.NHibernate
{
  public class SessionFactories
  {
    readonly static List<ISessionFactory> _factories = new List<ISessionFactory>();
    readonly static System.Threading.ReaderWriterLock _lock = new System.Threading.ReaderWriterLock();

    public static void Add(ISessionFactory sessionFactory)
    {
      using (RWLock.AsWriter(_lock))
      {
        if (!_factories.Contains(sessionFactory))
        {
          _factories.Add(sessionFactory);
        }
      }
    }

    public static void ForEach(Action<ISessionFactory> action)
    {
      using (RWLock.AsWriter(_lock))
      {
        foreach (var sessionFactory in _factories)
        {
          action(sessionFactory);
        }
      }
    }

    public static void Clear()
    {
      using (RWLock.AsWriter(_lock))
      {
        _factories.Clear();
      }
    }

    public static void UnbindAll()
    {
      ForEach(sf => {
        CurrentSessionContext.Unbind(sf);
      });
    }
  }
}