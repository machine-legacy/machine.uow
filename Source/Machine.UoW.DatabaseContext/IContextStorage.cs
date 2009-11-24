using System.Data;

namespace Machine.UoW.DatabaseContext
{
  public interface IContextStorage<T> where T : class
  {
    T Peek();
    void Push(T value);
    bool IsEmpty { get; }
    T Pop();
  }
}