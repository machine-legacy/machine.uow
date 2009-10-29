using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public interface IContextStorage<T> where T : class
  {
    T StoredValue { get; set;}
    bool HasValue { get; }
  }
}