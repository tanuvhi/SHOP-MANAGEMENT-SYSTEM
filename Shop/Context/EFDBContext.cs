using Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Shop.Context
{
    public class EFDBContext : DbContext
    {
     public   DbSet<Color> Colors { get; set; }
     public DbSet<Company> Companies { get; set; }
     public DbSet<OrderDetail> OrderDetails { get; set; }
     public DbSet<Order> Orders { get; set; }
     public DbSet<Product> Products { get; set; }
     public DbSet<Useer> Useers { get; set; }
     public DbSet<UserStatu> UserStatus { get; set; }
     public DbSet<ProductCompany> ProductCompanies { get; set; }
     public DbSet<ProductColor> ProductColors { get; set; }
     public DbSet<Stock> Stocks { get; set;}
     public DbSet<Purchase> Purchase { get; set; }

    }
}