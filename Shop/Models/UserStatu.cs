using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class UserStatu
    {
        [Key]
        public int StatusId { get; set; }
        public string Name { get; set; }
        public virtual List<Useer> Useers { get; set; }
    }
}