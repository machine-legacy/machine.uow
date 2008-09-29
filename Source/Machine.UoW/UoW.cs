using System;
using System.Collections.Generic;

namespace Machine.UoW
{
  public static class UoW
  {
    private static IUnitOfWorkProvider _provider;

    public static IUnitOfWorkProvider Provider
    {
      get { return _provider; }
      set { _provider = value; }
    }

    public static IUnitOfWork Current
    {
      get { return _provider.GetUnitOfWork(); }
    }

    public static IUnitOfWork Start()
    {
      return _provider.Start();
    }
  }
}
