using System;
using System.Data;

namespace Machine.UoW.AdoDotNet
{
  public interface IConnectionProvider
  {
    IDbConnection OpenConnection();
  }
}