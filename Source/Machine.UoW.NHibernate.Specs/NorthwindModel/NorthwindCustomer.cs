using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="Customers")]
  public class NorthwindCustomer
  {
    private long _customerId;
    private string _customerName;
    private IList<NorthwindOrder> _orders;

    [Id(TypeType=typeof(long))]
    [Generator(1, Class="native")]
    public virtual long CustomerId
    {
      get { return _customerId; }
      set { _customerId = value; }
    }

    [Property]
    public virtual string CustomerName
    {
      get { return _customerName; }
      set { _customerName = value; }
    }

    [OneToMany(ClassType=typeof(NorthwindOrder))]
    public virtual IList<NorthwindOrder> Orders
    {
      get { return _orders; }
      set { _orders = value; }
    }

    public override string ToString()
    {
      return String.Format("Customer<{0}>", this.CustomerId);
    }
  }
}
