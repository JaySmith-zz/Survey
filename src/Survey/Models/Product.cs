using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tyson.Hr.PayRules.Models
{
    public class Product
    {
        public virtual int ProductId { get; set; }
        public virtual int ProductCategoryId { get; set; }
        public virtual string ProductName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string color { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }

    public class ProductCategory
    {
        public virtual int ProductCategoryId { get; set; }
        public virtual string CategoryName { get; set; }
    }
}