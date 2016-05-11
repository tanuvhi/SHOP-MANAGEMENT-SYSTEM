using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int  UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public bool Delivery { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public  decimal PreviousBalance { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }

        public virtual Useer Useers { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}