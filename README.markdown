Machine.UoW
======================================================================

Example
-----------

Setting up and configurating for use with NHibernate under ASP.NET:

	ISessionFactory sessionFactory = ?;
	IUnitOfWorkManagement unitOfWorkManagement = new UnitOfWorkManagement();
	unitOfWorkManagement.AddEvents(new NHibernateUoWEvents(sessionFactory));
	IUnitOfWorkFactory factory = new UnitOfWorkFactory(unitOfWorkManagement);
	UoW.Provider = new HttpContextUnitOfWorkProvider(factory);

From then on, it's a matter of:

	using (IUnitOfWork uow = UoW.Start())
	{
		uow.Session().CreateQuery("FROM People").List<SessionType>();
	}

In this case Session() is an extension method provided by the 
Machine.UoW.NHibernate assembly. Machine.UoW has no dependency on NH.

Settings can be passed to the underlying UoW plugins:

	using (IUnitOfWork uow = UoW.Start(new NHibernateSessionSettings(IsolationLevel, FlushMode)))
	{
		uow.Session().CreateQuery("FROM People").List<SessionType>();
	}

Of course you can clean this up, etc... Machine.UoW.NH finds the settings and
uses them.
