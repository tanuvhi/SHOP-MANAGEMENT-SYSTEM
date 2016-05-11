using Shop.Context;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        EFDBContext context = new EFDBContext();
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult ProductSellReport()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
          return  View();
        }
        public ActionResult SellChart()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
       var od = context.OrderDetails.GroupBy(m=>m.Products.Name).Select(m=>new { ProductName = m.Key , Quantiy = m.Sum( w =>w.Quantity)});
       od = od.OrderByDescending(m => m.Quantiy);
       List<Product> model = new List<Product>();
            foreach(var i in od)
            {
                Product p = new Product();
                p.ProductId = (int) i.Quantiy;
                p.Name = i.ProductName;
                model.Add(p);
            }

            return View(model);
        }
        public ActionResult LifetimeMonthlyee()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult LifetimeMonthly() {
            var v = context.Orders.Where(i => i.OrderDate.HasValue)
                  .GroupBy(i => i.OrderDate.Value.Month)
                  .Select(g => new
                  {
                      Month = g.FirstOrDefault().OrderDate.Value.Month,
                      Total = g.Sum(i => i.TotalAmount)
                  });

            List<MonthlyValue> va = new List<MonthlyValue>();



            foreach (var m in v)
            {
                MonthlyValue val = new MonthlyValue();
                val.month = m.Month;
                val.sales = (int)m.Total;
                va.Add(val);

            }

            return View(va);
        }

        
        public ActionResult Salesin12Months() {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        public ActionResult MonthlySellChart()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var od = context.OrderDetails.Join(context.Orders,s=>s.OrderId,u=>u.OrderId, (s, u) => new { s, u }).Where(mm=>mm.u.OrderDate.Value.Year==2016).GroupBy(m => m.u.OrderDate.Value.Month).Select(m => new { OrderDate = m.Key, Amount = m.Sum(w => w.u.TotalAmount) });
            od = od.OrderBy(m => m.OrderDate);
            List<Product> model = new List<Product>();
            foreach (var i in od)
            {
                Product p = new Product();
                p.ProductId = (int)i.Amount;
                p.Name = i.OrderDate.ToString();
                model.Add(p);
            }

            return View(model);
          
        }
	}
}