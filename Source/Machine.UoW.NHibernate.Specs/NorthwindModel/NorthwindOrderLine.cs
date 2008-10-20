using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="OrderLines")]
  public class NorthwindOrderLine
  {
    private long _orderLineId;
    private NorthwindOrder _order;

    [Id(TypeType=typeof(long), Column="OrderLineId")]
    [Generator(1, Class="native")]
    public virtual long OrderLineId
    {
      get { return _orderLineId; }
      set { _orderLineId = value; }
    }

    [ManyToOne(ClassType=typeof(NorthwindOrder))]
    public virtual NorthwindOrder Order
    {
      get { return _order; }
      set { _order = value; }
    }

    public override string ToString()
    {
      return String.Format("OrderLine<{0}>", this.OrderLineId);
    }
  }
}
