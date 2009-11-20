using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Core.Utility;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class KeyedSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;
    readonly ReaderWriterLock _lock = new ReaderWriterLock();
    readonly Dictionary<object, ISession> _sessions = new Dictionary<object, ISession>();

    public KeyedSessionManager(ISessionFactory sessionFactory)
    {
      _sessionFactory = sessionFactory;
    }

    public IManagedSession OpenSession()
    {
      return OpenSession(String.Empty);
    }

    public virtual IManagedSession OpenSession(object key)
    {
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => NeedsNewSession(key)))
        {
          if (_sessions.ContainsKey(key))
          {
            _sessions[key].Dispose();
          }
          _sessions[key] = _sessionFactory.OpenSession();
        }
        return new ManagedSession(_sessions[key], false);
      }
    }

    private bool NeedsNewSession(object key)
    {
      if (!_sessions.ContainsKey(key))
        return true;
      return !_sessions[key].IsOpen;
    }

    public void DisposeAndRemoveSession(object key)
    {
      using (RWLock.AsWriter(_lock))
      { 
        if (_sessions.ContainsKey(key))
        {
          _sessions[key].Dispose();
          _sessions.Remove(key);
        }
      }
    }
  }
}