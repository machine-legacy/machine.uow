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
      throw new InvalidOperationException();
    }

    public IUnitOfWork GetUnitOfWork()
    {
      throw new InvalidOperationException();
    }
  }
}
