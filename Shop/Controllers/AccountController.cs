using Shop.Context;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        EFDBContext context = new EFDBContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Login(Useer model)
        {
            Useer user = context.Useers.Where(m=>m.PhoneNo == model.PhoneNo).Where(m=>m.Password== model.Password).FirstOrDefault();

            if(user==null)
            {
                 ModelState.AddModelError("", "Incorect Phone Number or password ");
            }else{
                if(user.StatusId==2)
                {
                    FormsAuthentication.SetAuthCookie(user.StatusId.ToString(), false);
                  

                    Session["StatusId"] = user.StatusId;
                    Session["Name"] = user.Name;

                    return RedirectToAction( "OrderList","Order");
                }
                else
                {

                    ModelState.AddModelError("", "You can't log in");
                }
            }
           
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
           
          
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Login", "Account");
        }
	}
}