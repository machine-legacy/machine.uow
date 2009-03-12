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
    }

    public void Dispose()
    {
      _session.Dispose();
    }
  }
}