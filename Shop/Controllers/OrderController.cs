using Shop.Context;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        private EFDBContext contex = new EFDBContext();


        public ActionResult OrderCreate()
        {
            if (Session["StatusId"] == null)
            {
              return  RedirectToAction("Login", "Account");
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult OrderCreate(int[] ItemId, decimal[] Quantiy, int[] Company,int[] Color ,decimal AmountPaid , decimal[] UnitPrice, decimal[] Total_Price, string OrderDate, Useer model)
        {

            decimal TotalAmoun=0;
            for (int i = 0; i < ItemId.Length; i++)
            {

                TotalAmoun += Total_Price[i];


            }

            string phone = model.PhoneNo;

            int use = contex.Useers.Where(v=>v.PhoneNo==phone).Select(m=>m.UserId).FirstOrDefault();
            if (use == 0)
            {
                Useer u = new Useer();
                model.StatusId = 1;
                u=contex.Useers.Add(model);
                contex.SaveChanges();
                int userid = u.UserId;


                Order order = new Order();
                order.UserId = userid;
                order.OrderDate = Convert.ToDateTime(OrderDate);
                order.Delivery = false;
                order.TotalAmount = TotalAmoun;
                order.Discount = 0;
                order.PreviousBalance = 0;
                order.AmountPaid = AmountPaid;
                order.Balance = TotalAmoun - AmountPaid;
                order = contex.Orders.Add(order);
                contex.SaveChanges();
                int orderid = order.OrderId;

                for (int i = 0; i < ItemId.Length; i++)
                {
                    OrderDetail od = new OrderDetail();
                    od.OrderId = orderid;
                    od.ProductId = ItemId[i];
                    od.Price = Total_Price[i];

                    od.Quantity = Quantiy[i];
                    if (Color[i]==0)
                    {
                        od.ColorId = 6;
                    }else
                    od.ColorId = Color[i];
                    if(Company[i]==0)
                    {
                        od.CompanyId = 6;
                    }else

                    od.CompanyId = Company[i];
                    od = contex.OrderDetails.Add(od);
                    contex.SaveChanges();
                    Stock st = contex.Stocks.Where(m=>m.ColorId== od.ColorId).Where(m=>m.ProductId==od.ProductId).Where(m=>m.CompanyId==od.CompanyId).FirstOrDefault();
                    if (st != null)
                    {
                        st = contex.Stocks.Find(st.StockId);
                        st.Quantity -= od.Quantity;
                        contex.SaveChanges();

                    }
                }


            }
            else
            {
                Order order = new Order();
                order.UserId = use;
                order.OrderDate = Convert.ToDateTime(OrderDate);
                order.Delivery = false;
                order.TotalAmount = TotalAmoun;
                order.Discount = 0;
                order.PreviousBalance = 0;
                order.AmountPaid = AmountPaid;
                order.Balance = TotalAmoun - AmountPaid;
                order = contex.Orders.Add(order);
                contex.SaveChanges();
                int orderid = order.OrderId;

                for (int i = 0; i < ItemId.Length; i++)
                {
                    OrderDetail od = new OrderDetail();
                    od.OrderId = orderid;
                    od.ProductId = ItemId[i];
                    od.Price = Total_Price[i];

                    od.Quantity = Quantiy[i];
                    if (Color[i] == 0)
                    {
                        od.ColorId = 6;
                    }
                    else
                        od.ColorId = Color[i];
                    if (Company[i] == 0)
                    {
                        od.CompanyId = 6;
                    }
                    else

                        od.CompanyId = Company[i];
                    od = contex.OrderDetails.Add(od);
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
            var PC = contex.ProductCompanies.Where(p=>p.ProductId== id).ToList();
            var C = contex.Companies.ToList();
           
            var result = (from p in PC
                          from c in C
                          where(c.CompanyId==p.CompanyId)
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
     
        public ActionResult  OrderList()
        {
              if (Session["StatusId"] == null)
            {
              return  RedirectToAction("Login", "Account");
            }
            List<Order> model = contex.Orders.ToList();
            return View(model);
        }
        public ActionResult OrderDetailsList(int id)
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<OrderDetail> model = contex.OrderDetails.Where(o=>o.OrderId==id).ToList();
            return View(model);
        }
        public ActionResult OrderDetailsEdit(int id)
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Color = new SelectList(contex.ProductColors.Where(m => m.ProductId == id).ToList(), "ColorId", "Colors.Name");
            ViewBag.Company = new SelectList(contex.ProductCompanies.Where(m => m.ProductId == id).ToList(), "CompanyId", "Companies.Name");
           OrderDetail model = contex.OrderDetails.Where(o => o.OrderDetailsId == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult OrderDetailsEdit(OrderDetail  orderDetail)
        {
            ViewBag.Color = new SelectList(contex.ProductColors.Where(m => m.ProductId == orderDetail.ProductId).ToList(), "ColorId", "Colors.Name");
            ViewBag.Company = new SelectList(contex.ProductCompanies.Where(m => m.ProductId == orderDetail.ProductId).ToList(), "CompanyId", "Companies.Name");

            OrderDetail OD = contex.OrderDetails.Find(orderDetail.OrderDetailsId);
         decimal price=   (OD.Price - orderDetail.Price)*-1;
         Order or = contex.Orders.Find(orderDetail.OrderId);
         or.TotalAmount += price;
         or.Balance = or.TotalAmount - or.AmountPaid;
         contex.SaveChanges();

            OD.OrderId = orderDetail.OrderId;
            OD.Price = orderDetail.Price;
            OD.ProductId = orderDetail.ProductId;
            OD.Quantity = orderDetail.Quantity;
            OD.ColorId = orderDetail.ColorId;
            OD.CompanyId = orderDetail.CompanyId;
            
            contex.SaveChanges();
            
            return View();
        }


        public ActionResult OrderDelete(int id)
        {
           Order order= contex.Orders.Find( id);

           List<OrderDetail> od= contex.OrderDetails.Where(o => o.OrderId == id).ToList();
            if(od != null )
            {
                foreach(var i  in  od)
                {
                    contex.OrderDetails.Remove(i);
                    contex.SaveChanges();
                }
            }
            
            if(order !=null)
            {
                contex.Orders.Remove(order);
                contex.SaveChanges();
            }
            
            return RedirectToAction("OrderList");
        }

	}
}