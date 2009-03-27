using System;
using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class CurrentSession : IDisposable
  {
    readonly ISession _session;

    public ISession Session
    {
      get { return _session; }
    }

    public CurrentSession(ISession session)
    {
      _session = session;
      NH.Session = session;
    }

    public void Dispose()
    {
      _session.Dispose();
      NH.Session = null;
    }
  }

  public static class NH
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(NH));
    [ThreadStatic]
    static ISession _session;

    public static ISession Session
    {
      get
      {
        if (_session != null)
        {
          return _session;
        }
        return UoW.Scope().Session();
      }
      set
      {
        if (_session != value)
        {
          if (_session != null && value != null)
          {
            _log.Info("Set " + value + " over " + _session);
            // throw new InvalidOperationException("Trying to use another Session when one is already in use!");
          }
        }
        else
        {
          _log.Info("Set " + value);
        }
        _session = value;
      }
    }
  }
}