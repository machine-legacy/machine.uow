using System;
using System.Collections;
using System.Data;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

namespace Machine.UoW.NHibernate
{
  #pragma warning disable 612,618
  public class SessionWrapper : ISession
  {
    readonly ISession _session;

    public SessionWrapper(ISession session)
    {
      _session = session;
    }

    public EntityMode ActiveEntityMode
    {
      get { return _session.ActiveEntityMode; }
    }

    public FlushMode FlushMode
    {
      get { return _session.FlushMode; }
      set { _session.FlushMode = value; }
    }

    public CacheMode CacheMode
    {
      get { return _session.CacheMode; }
      set { _session.CacheMode = value; }
    }

    public ISessionFactory SessionFactory
    {
      get { return _session.SessionFactory; }
    }

    public IDbConnection Connection
    {
      get { return _session.Connection; }
    }

    public bool IsOpen
    {
      get { return _session.IsOpen; }
    }

    public bool IsConnected
    {
      get { return _session.IsConnected; }
    }

    public global::NHibernate.ITransaction Transaction
    {
      get { return _session.Transaction; }
    }

    public ISessionStatistics Statistics
    {
      get { return _session.Statistics; }
    }

    public void Flush()
    {
      _session.Flush();
    }

    public IDbConnection Disconnect()
    {
      return _session.Disconnect();
    }

    public void Reconnect()
    {
      _session.Reconnect();
    }

    public virtual void Reconnect(IDbConnection connection)
    {
      _session.Reconnect(connection);
    }

    public IDbConnection Close()
    {
      return _session.Close();
    }

    public void CancelQuery()
    {
      _session.CancelQuery();
    }

    public bool IsDirty()
    {
      return _session.IsDirty();
    }

    public virtual object GetIdentifier(object obj)
    {
      return _session.GetIdentifier(obj);
    }

    public virtual bool Contains(object obj)
    {
      return _session.Contains(obj);
    }

    public virtual void Evict(object obj)
    {
      _session.Evict(obj);
    }

    public virtual object Load(Type theType, object id, LockMode lockMode)
    {
      return _session.Load(theType, id, lockMode);
    }

    public virtual object Load(string entityName, object id, LockMode lockMode)
    {
      return _session.Load(entityName, id, lockMode);
    }

    public virtual object Load(Type theType, object id)
    {
      return _session.Load(theType, id);
    }

    public virtual T Load<T>(object id, LockMode lockMode)
    {
      return _session.Load<T>(id, lockMode);
    }

    public virtual T Load<T>(object id)
    {
      return _session.Load<T>(id);
    }

    public virtual object Load(string entityName, object id)
    {
      return _session.Load(entityName, id);
    }

    public virtual void Load(object obj, object id)
    {
      _session.Load(obj, id);
    }

    public virtual void Replicate(object obj, ReplicationMode replicationMode)
    {
      _session.Replicate(obj, replicationMode);
    }

    public virtual void Replicate(string entityName, object obj, ReplicationMode replicationMode)
    {
      _session.Replicate(entityName, obj, replicationMode);
    }

    public virtual object Save(object obj)
    {
      return _session.Save(obj);
    }

    public virtual void Save(object obj, object id)
    {
      _session.Save(obj, id);
    }

    public virtual object Save(string entityName, object obj)
    {
      return _session.Save(entityName, obj);
    }

    public virtual void SaveOrUpdate(object obj)
    {
      _session.SaveOrUpdate(obj);
    }

    public virtual void SaveOrUpdate(string entityName, object obj)
    {
      _session.SaveOrUpdate(entityName, obj);
    }

    public virtual void Update(object obj)
    {
      _session.Update(obj);
    }

    public virtual void Update(object obj, object id)
    {
      _session.Update(obj, id);
    }

    public virtual void Update(string entityName, object obj)
    {
      _session.Update(entityName, obj);
    }

    public virtual object Merge(object obj)
    {
      return _session.Merge(obj);
    }

    public virtual object Merge(string entityName, object obj)
    {
      return _session.Merge(entityName, obj);
    }

    public virtual void Persist(object obj)
    {
      _session.Persist(obj);
    }

    public virtual void Persist(string entityName, object obj)
    {
      _session.Persist(entityName, obj);
    }

    public virtual object SaveOrUpdateCopy(object obj)
    {
      return _session.SaveOrUpdateCopy(obj);
    }

    public virtual object SaveOrUpdateCopy(object obj, object id)
    {
      return _session.SaveOrUpdateCopy(obj, id);
    }

    public virtual void Delete(object obj)
    {
      _session.Delete(obj);
    }

    public virtual void Delete(string entityName, object obj)
    {
      _session.Delete(entityName, obj);
    }

    public virtual IList Find(string query)
    {
      return _session.Find(query);
    }

    public virtual IList Find(string query, object value, IType type)
    {
      return _session.Find(query, value, type);
    }

    public virtual IList Find(string query, Object[] values, IType[] types)
    {
      return _session.Find(query, values, types);
    }

    public virtual IEnumerable Enumerable(string query)
    {
      return _session.Enumerable(query);
    }

    public virtual IEnumerable Enumerable(string query, object value, IType type)
    {
      return _session.Enumerable(query, value, type);
    }

    public virtual IEnumerable Enumerable(string query, Object[] values, IType[] types)
    {
      return _session.Enumerable(query, values, types);
    }

    public virtual ICollection Filter(object collection, string filter)
    {
      return _session.Filter(collection, filter);
    }

    public virtual ICollection Filter(object collection, string filter, object value, IType type)
    {
      return _session.Filter(collection, filter, value, type);
    }

    public virtual ICollection Filter(object collection, string filter, Object[] values, IType[] types)
    {
      return _session.Filter(collection, filter, values, types);
    }

    public virtual int Delete(string query)
    {
      return _session.Delete(query);
    }

    public virtual int Delete(string query, object value, IType type)
    {
      return _session.Delete(query, value, type);
    }

    public virtual int Delete(string query, Object[] values, IType[] types)
    {
      return _session.Delete(query, values, types);
    }

    public virtual void Lock(object obj, LockMode lockMode)
    {
      _session.Lock(obj, lockMode);
    }

    public virtual void Lock(string entityName, object obj, LockMode lockMode)
    {
      _session.Lock(entityName, obj, lockMode);
    }

    public virtual void Refresh(object obj)
    {
      _session.Refresh(obj);
    }

    public virtual void Refresh(object obj, LockMode lockMode)
    {
      _session.Refresh(obj, lockMode);
    }

    public virtual LockMode GetCurrentLockMode(object obj)
    {
      return _session.GetCurrentLockMode(obj);
    }

    public global::NHibernate.ITransaction BeginTransaction()
    {
      return _session.BeginTransaction();
    }

    public virtual global::NHibernate.ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      return _session.BeginTransaction(isolationLevel);
    }

    public virtual ICriteria CreateCriteria(Type persistentClass)
    {
      return _session.CreateCriteria(persistentClass);
    }

    public virtual ICriteria CreateCriteria(Type persistentClass, string alias)
    {
      return _session.CreateCriteria(persistentClass, alias);
    }

    public virtual ICriteria CreateCriteria(string entityName)
    {
      return _session.CreateCriteria(entityName);
    }

    public virtual ICriteria CreateCriteria(string entityName, string alias)
    {
      return _session.CreateCriteria(entityName, alias);
    }

    public virtual IQuery CreateQuery(string queryString)
    {
      return _session.CreateQuery(queryString);
    }

    public virtual IQuery CreateFilter(object collection, string queryString)
    {
      return _session.CreateFilter(collection, queryString);
    }

    public virtual IQuery GetNamedQuery(string queryName)
    {
      return _session.GetNamedQuery(queryName);
    }

    public virtual IQuery CreateSQLQuery(string sql, string returnAlias, Type returnClass)
    {
      return _session.CreateSQLQuery(sql, returnAlias, returnClass);
    }

    public virtual IQuery CreateSQLQuery(string sql, String[] returnAliases, Type[] returnClasses)
    {
      return _session.CreateSQLQuery(sql, returnAliases, returnClasses);
    }

    public virtual ISQLQuery CreateSQLQuery(string queryString)
    {
      return _session.CreateSQLQuery(queryString);
    }

    public void Clear()
    {
      _session.Clear();
    }

    public virtual object Get(Type clazz, object id)
    {
      return _session.Get(clazz, id);
    }

    public virtual object Get(Type clazz, object id, LockMode lockMode)
    {
      return _session.Get(clazz, id, lockMode);
    }

    public virtual object Get(string entityName, object id)
    {
      return _session.Get(entityName, id);
    }

    public virtual T Get<T>(object id)
    {
      return _session.Get<T>(id);
    }

    public virtual T Get<T>(object id, LockMode lockMode)
    {
      return _session.Get<T>(id, lockMode);
    }

    public virtual string GetEntityName(object obj)
    {
      return _session.GetEntityName(obj);
    }

    public virtual IFilter EnableFilter(string filterName)
    {
      return _session.EnableFilter(filterName);
    }

    public virtual IFilter GetEnabledFilter(string filterName)
    {
      return _session.GetEnabledFilter(filterName);
    }

    public virtual void DisableFilter(string filterName)
    {
      _session.DisableFilter(filterName);
    }

    public IMultiQuery CreateMultiQuery()
    {
      return _session.CreateMultiQuery();
    }

    public virtual ISession SetBatchSize(int batchSize)
    {
      return _session.SetBatchSize(batchSize);
    }

    public ISessionImplementor GetSessionImplementation()
    {
      return _session.GetSessionImplementation();
    }

    public IMultiCriteria CreateMultiCriteria()
    {
      return _session.CreateMultiCriteria();
    }

    public virtual ISession GetSession(EntityMode entityMode)
    {
      return _session.GetSession(entityMode);
    }

    public virtual void Dispose()
    {
      _session.Dispose();
    }

    public bool Equals(SessionWrapper other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other._session, _session);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(SessionWrapper)) return false;
      return Equals((SessionWrapper)obj);
    }

    public override Int32 GetHashCode()
    {
      return _session.GetHashCode();
    }
  }
  #pragma warning restore 612,618
}