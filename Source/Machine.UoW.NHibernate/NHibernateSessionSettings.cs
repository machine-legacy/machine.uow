using System;
using System.Data;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class NHibernateSessionSettings : IUnitOfWorkSettings
  {
    public static readonly NHibernateSessionSettings Default = new NHibernateSessionSettings(IsolationLevel.Unspecified, FlushMode.Unspecified);

    private readonly IsolationLevel _isolationLevel;
    private readonly FlushMode _flushMode;

    public IsolationLevel IsolationLevel
    {
      get { return _isolationLevel; }
    }

    public FlushMode FlushMode
    {
      get { return _flushMode; }
    }

    public NHibernateSessionSettings(IsolationLevel isolationLevel, FlushMode flushMode)
    {
      _isolationLevel = isolationLevel;
      _flushMode = flushMode;
    }

    public void Dispose()
    {
    }
  }
}