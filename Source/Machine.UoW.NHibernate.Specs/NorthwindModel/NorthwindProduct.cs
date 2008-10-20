using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="Products")]
  public class NorthwindProduct
  {
    private long _productId;
    private string _productName;
    private NorthwindCategory _category;

    [Id(TypeType=typeof(long), Column="ProductId")]
    [Generator(1, Class="native")]
    public virtual long ProductId
    {
      get { return _productId; }
      set { _productId = value; }
    }

    [Property]
    public virtual string ProductName
    {
      get { return _productName; }
      set { _productName = value; }
    }

    [ManyToOne(ClassType=typeof(NorthwindCategory))]
    public virtual NorthwindCategory Category
    {
      get { return _category; }
      set { _category = value; }
    }

    public override string ToString()
    {
      return String.Format("Product<{0}, {1}>", this.ProductId, this.ProductName);
    }
  }
}
