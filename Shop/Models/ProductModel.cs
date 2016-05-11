using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ProductModel
    {
        public Product product { get; set; }
        public IList<ProductColor> productColor { get; set; }
        public IList<ProductCompany> productCompany { get; set; }
        
    }
}