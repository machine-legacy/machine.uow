namespace Machine.UoW.AdoDotNet
{
  public class NullConnectionManager : IConnectionManager
  {
    public IManagedConnection OpenConnection(object key)
    {
      return NullManagedConnection.Null;
    }
  }

  public class NullManagedConnection : IManagedConnection
  {
    public static IManagedConnection Null = new NullManagedConnection();

    public void Rollback()
    {
    }

    public void Commit()
    {
    }

    public void Dispose()
    {
    }
  }
}