using System;
using System.Collections.Generic;
using System.IO;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;

using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  public class DatabaseAndNhSessionFactory
  {
    private DatabaseManager _database;
    private ISessionFactory _sessionFactory;

    public DatabaseManager Database
    {
      get { return _database; }
    }

    public ISessionFactory SessionFactory
    {
      get { return _sessionFactory; }
    }

    public void Open()
    {
      _database = new DatabaseManager();
      _database.Recreate();

      IDictionary<string, string> properties = SqlHelper.Provider.CreateNhibernateProperties();
      properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
      properties["connection.connection_string"] = _database.Connection.ConnectionString;
      properties["connection.release_mode"] = "on_close";
      properties["proxyfactory.factory_class"] = "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle";

      Configuration configuration = new Configuration();
      configuration.AddProperties(properties);
      HbmSerializer.Default.Validate = true;
      MemoryStream stream = HbmSerializer.Default.Serialize(typeof(NorthwindEmployee).Assembly);
      stream.Position = 0;
      configuration.AddInputStream(stream);
      stream.Close();

      _sessionFactory = configuration.BuildSessionFactory();
    }

    public void Close()
    {
      _sessionFactory.Close();
      _database.Close();
    }

    public void AddDefaultData()
    {
      NorthwindDatabase database = new NorthwindDatabase(_database.Connection);
      database.AddDefaultData();
    }

    public long AddSingleEmployee()
    {
      NorthwindDatabase database = new NorthwindDatabase(_database.Connection);
      return database.AddEmployee("Steve", "Zandt", DateTime.Now, true, 0);
    }
  }
}