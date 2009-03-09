using System;
using System.Data;

namespace Machine.UoW.SqlServer
{
  public interface IConnectionProvider
  {
    IDbConnection OpenConnection();
  }
}