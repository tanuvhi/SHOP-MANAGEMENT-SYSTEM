using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ProductCompany
    {
        [Key]
        public int ProductCompanyId { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }

        public virtual Product Products { get; set; }
        public virtual Company Companies { get; set; }
    }
}