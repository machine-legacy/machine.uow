using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="Orders")]
  public class NorthwindOrder
  {
    private long _orderId;
    private IList<NorthwindOrderLine> _orderLines;
    private NorthwindCustomer _customer;

    [Id(TypeType=typeof(long), Column="OrderId")]
    [Generator(1, Class="native")]
    public virtual long OrderId
    {
      get { return _orderId; }
      set { _orderId = value; }
    }

    [OneToMany(ClassType=typeof(NorthwindOrderLine))]
    public virtual IList<NorthwindOrderLine> OrderLines
    {
      get { return _orderLines; }
      set { _orderLines = value; }
    }

    [ManyToOne(ClassType=typeof(NorthwindCustomer))]
    public virtual NorthwindCustomer Customer
    {
      get { return _customer; }
      set { _customer = value; }
    }

    public override string ToString()
    {
      return String.Format("Order<{0}>", this.OrderId);
    }
  }
}