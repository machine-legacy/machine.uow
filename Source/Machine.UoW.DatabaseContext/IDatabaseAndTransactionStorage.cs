using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public interface IDatabaseAndTransactionStorage
  {
    IDbConnection Connection { get; set;}
    bool HasConnection { get; }
    IDbTransaction Transaction { get; set; }
    bool HasTransaction { get; }
  }
}