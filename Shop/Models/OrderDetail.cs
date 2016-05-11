using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }

        public int ColorId { get; set; }

        public int CompanyId { get; set; }
        public decimal Quantity { get; set; }
        public virtual Order Orders { get; set; }
        public virtual Product Products { get; set; }
        public virtual Company Companies { get; set; }
        public virtual Color Colors { get; set; }

    }
}