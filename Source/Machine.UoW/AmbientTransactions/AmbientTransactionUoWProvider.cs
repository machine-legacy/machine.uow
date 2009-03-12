using System;
using System.Collections.Generic;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUoWProvider : IUnitOfWorkProvider
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AmbientTransactionUoWProvider));
    readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public AmbientTransactionUoWProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      IUnitOfWork unitOfWork = state.Get<IUnitOfWork>();
      if (unitOfWork == null)
      {
        unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkFactory.StartScope(settings));
        state.Set<IUnitOfWork>(unitOfWork);
        _log.Info("Starting UoW");
        EnlistmentNotifications.Enlist(unitOfWork);
      }
      AmbientTransactionSettings ambientTransactionSettings = settings.AmbientSettings();
      return new AmbientTransactionUnitOfWorkProxy(unitOfWork, ambientTransactionSettings.ToScope());
    }

    public IUnitOfWork GetUnitOfWork()
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      return state.Get<IUnitOfWork>();
    }
  }
}
