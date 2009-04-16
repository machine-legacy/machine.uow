using System;

using NHibernate;

namespace Machine.UoW.NHibernate
{
  public class UnitOfWorkScopeSessionManager : ISessionManager
  {
    readonly ISessionFactory _sessionFactory;
    readonly IUnitOfWorkScopeProvider _unitOfWorkScopeProvider;

    public UnitOfWorkScopeSessionManager(ISessionFactory sessionFactory, IUnitOfWorkScopeProvider unitOfWorkScopeProvider)
    {
      _sessionFactory = sessionFactory;
      _unitOfWorkScopeProvider = unitOfWorkScopeProvider;
    }

    public IManagedSession OpenSession(object key)
    {
      IUnitOfWorkScope scope = _unitOfWorkScopeProvider.GetUnitOfWorkScope();
      ISession session = scope.Get(key, () => {
        return _sessionFactory.OpenSession();
      });
      return new ManagedSession(session, false);
    }

    public void DisposeAndRemoveSession(object key)
    {
      throw new InvalidOperationException("Cannot dispose and remove from UnitOFWorkScopeSessionManager");
    }
  }
}