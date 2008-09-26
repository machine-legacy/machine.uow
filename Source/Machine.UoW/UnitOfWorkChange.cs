using System;

namespace Machine.UoW
{
  public enum ChangeType
  {
    Added,
    Updated,
    Deleted
  }
  public class UnitOfWorkChange
  {
    private readonly UnitOfWorkEntry _entry;
    private readonly ChangeType _changeType;

    public UnitOfWorkEntry Entry
    {
      get { return _entry; }
    }

    public ChangeType ChangeType
    {
      get { return _changeType; }
    }

    public UnitOfWorkChange(UnitOfWorkEntry entry, ChangeType changeType)
    {
      _entry = entry;
      _changeType = changeType;
    }
  }
}