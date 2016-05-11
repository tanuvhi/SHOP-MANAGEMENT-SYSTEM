using Shop.Context;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    
    public class ProductController : Controller
    {

        EFDBContext contex = new EFDBContext();
        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult AddNewProduct()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Color = new SelectList(contex.Colors.ToList(), "ColorId", "Name");
            ViewBag.Company = new SelectList(contex.Companies.ToList(), "CompanyId", "Name");
       
            return View();
        }

        [HttpPost]
        public ActionResult AddNewProduct(Product model  , int[] company ,int[] color  )
        {
            ViewBag.Color = new SelectList(contex.Colors.ToList(), "ColorId", "Name");
            ViewBag.Company = new SelectList(contex.Companies.ToList(), "CompanyId", "Name");

            int c = contex.Products.Where(C => C.Name == model.Name).Select(s => s.ProductId).FirstOrDefault();
         
            if (c > 0)
            {
                TempData["UnsuccessMessageProduct"] = "This Product Name Already Added ";
            }
            else if (ModelState.IsValid)
            {

                Product product = contex.Products.Add(model);

                contex.SaveChanges();
                TempData["SuccessMessageProduct"] = "Product Added Successful ";
                if (company != null)
                {
                    foreach (var i in company)
                    {
                        ProductCompany productCompany = new ProductCompany();
                        productCompany.ProductId = product.ProductId;
                        productCompany.CompanyId = i;
                        contex.ProductCompanies.Add(productCompany);
                        contex.SaveChanges();
                    }
                }
                if (color != null)
                {
                    foreach (var i in color)
                    {
                        ProductColor clr = new ProductColor();
                        clr.ProductId = product.ProductId;
                        clr.ColorId = i;
                        contex.ProductColors.Add(clr);
                        contex.SaveChanges();
                    }
                }
            }
            return View(model);
        }
        public ActionResult AddNewCompany()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            

            return View();
        }
        [HttpPost]
        public ViewResult AddNewCompany(Company model)
        {
            
            int c = contex.Companies.Where(C => C.Name == model.Name).Select(s => s.CompanyId).FirstOrDefault();
            if (c > 0)
            {
                TempData["UnsuccessMessageCompany"] = "This Company Name Already Added ";
            }
        else   if (ModelState.IsValid)
            {
                contex.Companies.Add(model);
                contex.SaveChanges();
                TempData["SuccessMessageCompany"] = "Company Name Added Successful ";


            }
            return View();
        }
        public ActionResult AddNewColor()
        {
            if (Session["StatusId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }


            return View();
        }
        [HttpPost]
        public ActionResult AddNewColor(Color model)
        {
           int c= contex.Colors.Where(C=>C.Name==model.Name).Select(s=>s.ColorId).FirstOrDefault();
           if (c > 0)
           {
               TempData["UnsuccessMessageColor"] = "This Color Name Already Added ";
           }
          else  if (ModelState.IsValid)
            {
                contex.Colors.Add(model);
                contex.SaveChanges();
                TempData["SuccessMessageColor"]= "Color Name Added Successful ";
    
                
            }


            return View();
        }
            public ActionResult ProductList()
                {
                    if (Session["StatusId"] == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    List<Product> productList = contex.Products.ToList();

                    return View(productList);
                }
            public ActionResult ProductDetails(int id)
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                ProductModel model = new ProductModel();
                model.product = contex.Products.Where(m=>m.ProductId== id).FirstOrDefault();
                model.productCompany = contex.ProductCompanies.Where(m => m.ProductId == id).ToList();
                model.productColor = contex.ProductColors.Where(m => m.ProductId == id).ToList();
                return View(model);
            }
            public ActionResult  ColorList()
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                List<Color> model = contex.Colors.ToList();

                return View(model);
            }


            public ActionResult ColorEdit(int id)
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
               Color model = contex.Colors.FirstOrDefault(m=>m.ColorId==id);

                return View(model);
            }
            [HttpPost]
            public ViewResult ColorEdit(Color model)
            {
                Color c = contex.Colors.Find( model.ColorId);
                c.Name = model.Name;
                contex.SaveChanges();

                return View(model);
            }
            public ActionResult CompanyList()
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                List<Company> model = contex.Companies.ToList();

                return View(model);
            }
            public ActionResult CompanyEdit(int id)
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Company model = contex.Companies.FirstOrDefault(m => m.CompanyId == id);

                return View(model);
            }

            [HttpPost]
            public ViewResult CompanyEdit(Company model)
            {
                Company c = contex.Companies.Find(model.CompanyId);
                c.Name = model.Name;
                contex.SaveChanges();

                return View(model);
            }

            public ActionResult ProductEdit(int id)
            {
                if (Session["StatusId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ProductModel model = new ProductModel();
                 model.product = contex.Products.FirstOrDefault(m => m.ProductId == id);
              // model.productColor = contex.
                 return View();

            }

            
	}
}