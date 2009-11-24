namespace Machine.UoW.AdoDotNet
{
  public interface IConnectionManager
  {
    IManagedConnection OpenConnection(object key);
  }
}