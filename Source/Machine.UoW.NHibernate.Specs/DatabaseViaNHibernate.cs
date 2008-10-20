using System.Collections.Generic;
using System.IO;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;

using Machine.UoW.NHibernate.Specs.NorthwindModel;

namespace Machine.UoW.NHibernate.Specs
{
  public class DatabaseViaNHibernate
  {
    private readonly SqliteDatabase _database;
    private readonly ISessionFactory _sessionFactory;

    public SqliteDatabase Database
    {
      get { return _database; }
    }

    public ISessionFactory SessionFactory
    {
      get { return _sessionFactory; }
    }

    public DatabaseViaNHibernate()
    {
      _database = new SqliteDatabase();
      _database.Open();

      Dictionary<string, string> properties = new Dictionary<string, string>();
      properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
      properties["connection.driver_class"] = "NHibernate.Driver.SQLite20Driver";
      properties["connection.connection_string"] = _database.Connection.ConnectionString;
      properties["dialect"] = "NHibernate.Dialect.SQLiteDialect";

      Configuration configuration = new Configuration();
      configuration.AddProperties(properties);
      HbmSerializer.Default.Validate = true;
      MemoryStream stream = HbmSerializer.Default.Serialize(typeof(NorthwindEmployee).Assembly);
      stream.Position = 0;
      configuration.AddInputStream(stream);
      stream.Close();

      _sessionFactory = configuration.BuildSessionFactory();
    }
  }
}