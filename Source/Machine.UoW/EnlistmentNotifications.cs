using System;
using System.Collections.Generic;
using System.Transactions;

namespace Machine.UoW
{
  public class EnlistmentNotifications : IEnlistmentNotification
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(EnlistmentNotifications));
    readonly IUnitOfWork _unitOfWork;

    public static bool Enlist(IUnitOfWork unitOfWork)
    {
      Transaction transaction = Transaction.Current;
      if (transaction == null)
      {
        return false;
      }
      EnlistmentNotifications notifications = new EnlistmentNotifications(unitOfWork);
      transaction.EnlistVolatile(notifications, EnlistmentOptions.None);
      return true;
    }

    protected EnlistmentNotifications(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public void Commit(Enlistment enlistment)
    {
      _log.Info("Commit: " + _unitOfWork.WasCommitted + " " + _unitOfWork);
      enlistment.Done();
    }

    public void InDoubt(Enlistment enlistment)
    {
      _log.Info("InDoubt: " + _unitOfWork.WasCommitted + " " + _unitOfWork);
      Rollback(enlistment);
    }

    public void Prepare(PreparingEnlistment preparingEnlistment)
    {
      _log.Info("Prepare: " + _unitOfWork.WasCommitted + " " + _unitOfWork);
      _unitOfWork.Commit();
      preparingEnlistment.Prepared();
    }

    public void Rollback(Enlistment enlistment)
    {
      _log.Info("Rollback: " + _unitOfWork.WasCommitted + " " + _unitOfWork);
      _unitOfWork.Rollback();
      enlistment.Done();
    }
  }
}
