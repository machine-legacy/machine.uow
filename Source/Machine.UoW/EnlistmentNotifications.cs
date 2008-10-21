using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW
{
  public class EnlistmentNotifications : IEnlistmentNotification
  {
    private readonly IUnitOfWork _unitOfWork;

    public EnlistmentNotifications(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    #region IEnlistmentNotification Members
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
      _unitOfWork.Commit();
      preparingEnlistment.Prepared();
    }

    public void Rollback(Enlistment enlistment)
    {
      _unitOfWork.Rollback();
      enlistment.Done();
    }
    #endregion

    public static bool Enlist(IUnitOfWork unitOfWork)
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
  }
}
