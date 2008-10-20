using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="Employees")]
  public class NorthwindEmployee
  {
    private long _employeeId;
    private string _firstName;
    private string _lastName;
    private DateTime _birthdate;
    private bool _isAlive;
    private NorthwindEmployee _reportsTo;

    [Id(TypeType=typeof(long), Column="EmployeeId")]
    [Generator(1, Class="native")]
    public virtual long EmployeeId
    {
      get { return _employeeId; }
      set { _employeeId = value; }
    }

    [Property]
    public virtual string FirstName
    {
      get { return _firstName; }
      set { _firstName = value; }
    }

    [Property]
    public virtual string LastName
    {
      get { return _lastName; }
      set { _lastName = value; }
    }

    [Property]
    public virtual DateTime BirthDate
    {
      get { return _birthdate; }
      set { _birthdate = value; }
    }

    [Property]
    public virtual bool IsAlive
    {
      get { return _isAlive; }
      set { _isAlive = value; }
    }

    [ManyToOne(ClassType=typeof(NorthwindEmployee))]
    public virtual NorthwindEmployee ReportsTo
    {
      get { return _reportsTo; }
      set { _reportsTo = value; }
    }

    public override string ToString()
    {
      return String.Format("Employee<{0}, {1} {2}, {3}>", this.EmployeeId, this.FirstName, this.LastName, this.IsAlive);
    }
  }
}