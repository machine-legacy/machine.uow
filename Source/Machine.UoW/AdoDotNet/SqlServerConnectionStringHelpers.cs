using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Machine.UoW.AdoDotNet
{
  // Should maybe split the string on ; and look at the individual parts?
  public static class SqlServerConnectionStringHelpers
  {
    static readonly Regex _transactionsRe = new Regex(@"ENLIST\s*=\s*(\S+)", RegexOptions.IgnoreCase);
    static readonly Regex _poolingRe = new Regex(@"POOLING\s*=\s*(\S+)", RegexOptions.IgnoreCase);
    static readonly Regex _catalogRe = new Regex(@"INITIAL\s+CATALOG=(\w+);", RegexOptions.IgnoreCase);

    public static string DisableAutomaticEnlistment(string connectionString)
    {
      if (_transactionsRe.IsMatch(connectionString))
      {
        throw new InvalidOperationException("Already have ENLIST flag in connection string!");
      }
      return connectionString + ";ENLIST=FALSE";
    }

    public static string DisableConnectionPooling(string connectionString)
    {
      if (_poolingRe.IsMatch(connectionString))
      {
        throw new InvalidOperationException("Already have POOLING flag in connection string!");
      }
      return connectionString + ";POOLING=FALSE";
    }

    public static string GetDatabaseName(string connectionString)
    {
      Match match = _catalogRe.Match(connectionString);
      if (match.Success)
      {
        return match.Groups[1].Value;
      }
      return null;
    }
  }
}
