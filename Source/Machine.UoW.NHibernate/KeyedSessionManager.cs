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

    public virtual IManagedSession OpenSession(object key)
    {
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => !_sessions.ContainsKey(key)))
        {
          _sessions[key] = _sessionFactory.OpenSession();
        }
        return new ManagedSession(_sessions[key], false);
      }
    }
  }
}