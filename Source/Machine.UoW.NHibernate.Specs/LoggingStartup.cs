using System;
using System.Collections.Generic;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace Machine.UoW.NHibernate.Specs
{
  public class LoggingStartup
  {
    static bool _configured;

    public void Start()
    {
      if (_configured)
      {
        return;
      }
      PatternLayout layout = new PatternLayout(@"%-5p (%30.30c) %m%n");
      ConsoleAppender consoleAppender = new ConsoleAppender();
      consoleAppender.Layout = layout;
      OutputDebugStringAppender outputDebugStringAppender = new OutputDebugStringAppender();
      outputDebugStringAppender.Layout = layout;
      log4net.Config.BasicConfigurator.Configure(consoleAppender);

      ChangeLevel("NHibernate.Cfg", Level.Warn);
      ChangeLevel("NHibernate.Dialect", Level.Warn);
      ChangeLevel("NHibernate.Loader.Entity.AbstractEntityLoader", Level.Warn);
      ChangeLevel("NHibernate.Tuple.Entity.AbstractEntityTuplizer", Level.Warn);
      ChangeLevel("NHibernate.Persister.Entity.AbstractEntityPersister", Level.Warn);

      ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.AddAppender(outputDebugStringAppender);

      _configured = true;
    }

    public static void ChangeLevel(string logger, Level level)
    {
      ILog log = LogManager.GetLogger(logger);
      ((log4net.Repository.Hierarchy.Logger)(log.Logger)).Level = level;
    }
  }
}
