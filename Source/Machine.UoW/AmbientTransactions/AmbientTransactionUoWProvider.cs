using System;
using System.Collections.Generic;

namespace Machine.UoW.AmbientTransactions
{
  public class AmbientTransactionUoWProvider : IUnitOfWorkProvider
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AmbientTransactionUoWProvider));
    readonly IUnitOfWorkFactory _unitOfWorkFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public AmbientTransactionUoWProvider(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IUnitOfWork Start(IUnitOfWorkSettings[] settings)
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      IUnitOfWork unitOfWork = state.Get<IUnitOfWork>();
      if (unitOfWork == null)
      {
        unitOfWork = _unitOfWorkFactory.StartUnitOfWork(_unitOfWorkScopeProvider.GetUnitOfWorkScope(settings));
        state.Set<IUnitOfWork>(unitOfWork);
        _log.Info("Starting UoW");
        EnlistmentNotifications.Enlist(unitOfWork);
      }
      AmbientTransactionSettings ambientTransactionSettings = settings.AmbientSettings();
      return new AmbientTransactionUnitOfWorkProxy(unitOfWork, ambientTransactionSettings.ToScope());
    }

    public IUnitOfWork GetUnitOfWork()
    {
      if (!AmbientTransactionHelpers.InAmbientTransaction())
      {
        throw new InvalidOperationException("Ambient transaction scope invalid unless inside transaction!");
      }
      return AmbientTransactionUnitOfWorkProxy.Active;
    }
  }

  public class AmbientTransactionUnitOfWorkScopeProvider : IUnitOfWorkScopeProvider
  {
    readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public AmbientTransactionUnitOfWorkScopeProvider(IUnitOfWorkFactory unitOfWorkFactory)
    {
      _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IUnitOfWorkScope GetUnitOfWorkScope(params IUnitOfWorkSettings[] settings)
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      IUnitOfWorkScope scope = state.Get<IUnitOfWorkScope>();
      if (scope == null)
      {
        scope = _unitOfWorkFactory.StartScope(settings);
        scope.Disposed += OnUnitOfWorkScopeDisposed;
        state.Set<IUnitOfWorkScope>(scope);
      }
      return scope;
    }

    private static void OnUnitOfWorkScopeDisposed(object sender, EventArgs e)
    {
      TransactionState state = TransactionState.ForCurrentTransaction();
      state.Set<IUnitOfWorkScope>();
    }
  }
}
