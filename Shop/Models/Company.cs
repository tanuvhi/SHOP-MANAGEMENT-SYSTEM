using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
  
        public virtual List<ProductCompany> ProductCompanies { get; set; }
        public virtual List<Purchase> Purchases { get; set; }
        public virtual List<Stock> Stocks { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}