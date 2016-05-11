using Shop.Context;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class PurchaseController : Controller
    {
        private EFDBContext contex = new EFDBContext();
        //
        // GET: /Purchase/
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult PurchaseItem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PurchaseItem(int[] ItemId, decimal[] Quantiy, int[] Company, int[] Color, decimal[] UnitPrice, decimal[] Total_Price, string PurchaseDate)
        {
            for (int i = 0; i < ItemId.Length;i++ )
            {
                int existance = contex.Stocks.Where(m => m.ProductId == ItemId[i]).Where(m=>m.ColorId== Color[i]).Where(m=>m.CompanyId == Company[i]).Select(n=>n.StockId).FirstOrDefault();
                if (existance != 0)
                {
                    Purchase p = new Purchase();
                    p.PurchaseDate = Convert.ToDateTime(PurchaseDate);
                    p.ProductId=ItemId[i];
                    p.Quantity=Quantiy[i];
                    p.UnitPrice=UnitPrice[i];
                    p.CompanyId = Company[i];
                    p.ColorId = Color[i];
                    contex.Purchase.Add(p);
                    contex.SaveChanges();

                    Stock st = new Stock();
                    st.ProductId = ItemId[i];
                    st.Quantity = Quantiy[i];
                    st.CompanyId = Company[i];
                    st.ColorId=Color[i];
                    contex.Stocks.Add(st);
                    contex.SaveChanges();
                }
                else {
                    Stock s = contex.Stocks.Find( existance); //from stock in contex.Stocks where stock.StockId == existance select stock; //contex.Stocks.FirstOrDefault(m=>m.StockId==existance);
                    s.Quantity += Quantiy[i];
                    contex.SaveChanges();

                    Purchase p = new Purchase();
                    p.PurchaseDate = Convert.ToDateTime(PurchaseDate);
                    p.ProductId = ItemId[i];
                    p.Quantity = Quantiy[i];
                    p.UnitPrice = UnitPrice[i];
                    p.CompanyId = Company[i];
                    p.ColorId = Color[i];
                    contex.Purchase.Add(p);
                    contex.SaveChanges();

                }
            }
            

           
            return View();
        }

        public JsonResult PNameAutoComplite(string term)
        {
            var result = from u in contex.Products
                         where u.Name.ToLower().Contains(term.ToLower())
                         select new { u.Name, u.ProductId };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCompany(string productId)
        {
            if (String.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException("productId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(productId, out id);
            var PC = contex.ProductCompanies.Where(p => p.ProductId == id).ToList();
            var C = contex.Companies.ToList();

            var result = (from p in PC
                          from c in C
                          where (c.CompanyId == p.CompanyId)
                          select new
                          {
                              id = c.CompanyId,
                              name = c.Name
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetColor(string productId)
        {
            if (String.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException("productId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(productId, out id);
            var PC = contex.ProductColors.Where(p => p.ProductId == id).ToList();
            var C = contex.Colors.ToList();

            var result = (from p in PC
                          from c in C
                          where (c.ColorId == p.ColorId)
                          select new
                          {
                              id = c.ColorId,
                              name = c.Name
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        } 

        public ViewResult PurchaseList()
        {
            List<Purchase> model = contex.Purchase.OrderBy(m=>m.PurchaseDate).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult PurchaseList( string d1, string d2)
        {
            if (d1 == null || d2 == null)
            {
                d1 = "1-2-2015";
                d2 = "1-2-2015";
            }
            DateTime dt1 =  DateTime.Parse( d1);
            DateTime dt2 = DateTime.Parse(d2);
            List<Purchase> model = contex.Purchase.OrderBy(m => m.PurchaseDate).Where(p => p.PurchaseDate  >= dt1).Where(p => p.PurchaseDate  <=  dt2).ToList();
            return View(model);
        }

        public ViewResult StockList()
        {
            List<Stock> model = contex.Stocks.ToList();
            return View(model);
        }
	}
}