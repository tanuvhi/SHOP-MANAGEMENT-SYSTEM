using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Useer
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }
        public int StatusId { get; set; }
        public virtual UserStatu UserStatus { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}