using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW
{
  public class EnlistmentNotifications : IEnlistmentNotification
  {
    readonly UnitOfWork _unitOfWork;

    public static bool Enlist(UnitOfWork unitOfWork)
    {
      Transaction transaction = Transaction.Current;
      if (transaction == null)
      {
        return false;
      }
      EnlistmentNotifications notifications = new EnlistmentNotifications(unitOfWork);
      transaction.EnlistVolatile(notifications, EnlistmentOptions.None);
      unitOfWork.Set(notifications);
      return true;
    }

    public EnlistmentNotifications(UnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public void Commit(Enlistment enlistment)
    {
      enlistment.Done();
    }

    public void InDoubt(Enlistment enlistment)
    {
      Rollback(enlistment);
    }

    public void Prepare(PreparingEnlistment preparingEnlistment)
    {
      _unitOfWork.Commit(CommitOrRollbackType.Ambient);
      preparingEnlistment.Prepared();
    }

    public void Rollback(Enlistment enlistment)
    {
      _unitOfWork.Rollback(CommitOrRollbackType.Ambient);
      enlistment.Done();
    }
  }
}
