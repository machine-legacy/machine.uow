using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

using Machine.Specifications;
using Machine.Specifications.Utility;
using Machine.UoW.AdoDotNet;
using Machine.UoW.DatabaseContext;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Stat;
using Rhino.Mocks;

namespace Machine.UoW.NHibernate.ManagerSpecs
{
  [Subject("Connection and Session Manager")]
  public class when_using_a_session : ManagerSpecs
  {
    static ISession s1;

    Because of = () =>
    {
      using (var scope = new TransactionScope())
      {
        using (var session = PrimaryDatabase.OpenSession())
        {
          s1 = NH.Session;
          session.Commit();
        }
        scope.Complete();
      }
    };

    It should_use_a_session = () =>
      s1.ShouldNotBeNull();
  }

  [Subject("Connection and Session Manager")]
  public class when_using_a_session_twice : ManagerSpecs
  {
    static ISession s1;
    static ISession s2;

    Because of = () =>
    {
      using (var scope = new TransactionScope())
      {
        using (var session = PrimaryDatabase.OpenSession())
        {
          s1 = NH.Session;
          session.Commit();
        }
        using (var session = PrimaryDatabase.OpenSession())
        {
          s2 = NH.Session;
          session.Commit();
        }
        scope.Complete();
      }
    };

    It should_use_a_session = () =>
      s1.ShouldNotBeNull();

    It should_use_one_session = () =>
      s1.ShouldEqual(s2);
  }

  [Subject("Connection and Session Manager")]
  public class when_using_a_connection : ManagerSpecs
  {
    static IDbConnection c1;

    Because of = () =>
    {
      using (var scope = new TransactionScope())
      {
        using (var connection = PrimaryDatabase.OpenConnection())
        {
          c1 = Database.Connection;
        }
        scope.Complete();
      }
    };

    It should_use_a_connection = () =>
      c1.ShouldNotBeNull();
  }

  [Subject("Connection and Session Manager")]
  public class when_using_a_connection_twice : ManagerSpecs
  {
    static IDbConnection c1;
    static IDbConnection c2;

    Because of = () =>
    {
      using (var scope = new TransactionScope())
      {
        using (var connection = PrimaryDatabase.OpenConnection())
        {
          c1 = Database.Connection;
        }
        using (var connection = PrimaryDatabase.OpenConnection())
        {
          c2 = Database.Connection;
        }
        scope.Complete();
      }
    };

    It should_use_a_connection = () =>
      c1.ShouldNotBeNull();

    It should_use_one_connection = () =>
      c1.ShouldEqual(c2);
  }

  public class ManagerSpecs
  {
    protected static ISessionFactory sessionFactory;
    protected static ISession session1;
    protected static ISession session2;
    protected static IDbConnection connection1;
    protected static IDbConnection connection2;
    protected static global::NHibernate.ITransaction transaction1;
    protected static global::NHibernate.ITransaction transaction2;

    Establish context = () =>
    {
      session1 = MockRepository.GenerateMock<ISession>();
      session2 = MockRepository.GenerateMock<ISession>();
      connection1 = MockRepository.GenerateMock<IDbConnection>();
      connection2 = MockRepository.GenerateMock<IDbConnection>();
      transaction1 = MockRepository.GenerateMock<global::NHibernate.ITransaction>();
      transaction2 = MockRepository.GenerateMock<global::NHibernate.ITransaction>();
      session1.Expect(x => x.BeginTransaction()).Return(transaction1);
      session1.Expect(x => x.BeginTransaction()).Return(transaction1);
      session2.Expect(x => x.BeginTransaction()).Return(transaction2);
      session1.Expect(x => x.Connection).Return(connection1);
      session1.Expect(x => x.Connection).Return(connection1);
      session2.Expect(x => x.Connection).Return(connection2);
      sessionFactory = new MockSessionFactory(session1, session2);
      PrimaryDatabase.Startup(sessionFactory);
    };

    Cleanup after = () =>
    {
    };
  }

  public static class PrimaryDatabase
  {
    private static ISessionManager _sessionManager;
    private static IConnectionManager _connectionManager;
    private static IConnectionProvider _connectionProvider;

    public static IManagedSession OpenSession()
    {
      return _sessionManager.OpenSession();
    }

    public static IManagedConnection OpenConnection()
    {
      return _connectionManager.OpenConnection(String.Empty);
    }

    public static void Startup(ISessionFactory sessionFactory)
    {
      _connectionProvider = new MockConnectionProvider(); 
      _sessionManager = new TransientSessionManager(sessionFactory);
      _connectionManager = new TransientConnectionManager(_connectionProvider);
    }
  }

  public class MockConnectionProvider : IConnectionProvider
  {
    public IDbConnection OpenConnection()
    {
      return MockRepository.GenerateMock<IDbConnection>();
    }
  }

  public class MockSessionFactory : ISessionFactory
  {
    readonly Queue<ISession> _sessions = new Queue<ISession>();

    public MockSessionFactory(params ISession[] sessions)
    {
      sessions.Each(x => _sessions.Enqueue(x));
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public ISession OpenSession(IDbConnection conn)
    {
      throw new NotImplementedException();
    }

    public ISession OpenSession(IInterceptor sessionLocalInterceptor)
    {
      throw new NotImplementedException();
    }

    public ISession OpenSession(IDbConnection conn, IInterceptor sessionLocalInterceptor)
    {
      throw new NotImplementedException();
    }

    public ISession OpenSession()
    {
      return _sessions.Dequeue();
    }

    public IClassMetadata GetClassMetadata(Type persistentClass)
    {
      throw new NotImplementedException();
    }

    public IClassMetadata GetClassMetadata(string entityName)
    {
      throw new NotImplementedException();
    }

    public ICollectionMetadata GetCollectionMetadata(string roleName)
    {
      throw new NotImplementedException();
    }

    public IDictionary<string, IClassMetadata> GetAllClassMetadata()
    {
      throw new NotImplementedException();
    }

    public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
    {
      throw new NotImplementedException();
    }

    public void Close()
    {
      throw new NotImplementedException();
    }

    public void Evict(Type persistentClass)
    {
      throw new NotImplementedException();
    }

    public void Evict(Type persistentClass, object id)
    {
      throw new NotImplementedException();
    }

    public void EvictEntity(string entityName)
    {
      throw new NotImplementedException();
    }

    public void EvictEntity(string entityName, object id)
    {
      throw new NotImplementedException();
    }

    public void EvictCollection(string roleName)
    {
      throw new NotImplementedException();
    }

    public void EvictCollection(string roleName, object id)
    {
      throw new NotImplementedException();
    }

    public void EvictQueries()
    {
      throw new NotImplementedException();
    }

    public void EvictQueries(string cacheRegion)
    {
      throw new NotImplementedException();
    }

    public IStatelessSession OpenStatelessSession()
    {
      throw new NotImplementedException();
    }

    public IStatelessSession OpenStatelessSession(IDbConnection connection)
    {
      throw new NotImplementedException();
    }

    public FilterDefinition GetFilterDefinition(string filterName)
    {
      throw new NotImplementedException();
    }

    public ISession GetCurrentSession()
    {
      throw new NotImplementedException();
    }

    public IStatistics Statistics
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsClosed
    {
      get { throw new NotImplementedException(); }
    }

    public ICollection<string> DefinedFilterNames
    {
      get { throw new NotImplementedException(); }
    }
  }
}
