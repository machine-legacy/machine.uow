using System;
using System.Collections.Generic;

using NHibernate.Mapping.Attributes;

namespace Machine.UoW.NHibernate.Specs.NorthwindModel
{
  [Class(Table="Categories")]
  public class NorthwindCategory
  {
    private long _categoryId;
    private string _categoryName;
    private IList<NorthwindProduct> _products;

    [Id(TypeType=typeof(long), Column="CategoryId")]
    [Generator(1, Class="native")]
    public virtual long CategoryId
    {
      get { return _categoryId; }
      set { _categoryId = value; }
    }

    [Property]
    public virtual string CategoryName
    {
      get { return _categoryName; }
      set { _categoryName = value; }
    }

    [OneToMany(ClassType=typeof(NorthwindProduct))]
    public virtual IList<NorthwindProduct> Products
    {
      get { return _products; }
      set { _products = value; }
    }

    public override string ToString()
    {
      return String.Format("Category<{0}, {1}>", this.CategoryId, this.CategoryName);
    }
  }
}