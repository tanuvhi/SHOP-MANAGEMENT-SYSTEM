using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int CompanyId { get; set; }
        public int ColorId { get; set; }
        public virtual Product Products { get; set; }
        public virtual Company Companies { get; set; }
        public virtual Color Colors { get; set; }
    }
}