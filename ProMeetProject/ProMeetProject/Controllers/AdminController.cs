using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProMeetProject.Models;

namespace ProMeetProject.Controllers
{
    public class AdminController : Controller
    {
        private FinalDatabaseEntities1 db = new FinalDatabaseEntities1();
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin objUser)
        {
            if (ModelState.IsValid)
            {
                using (FinalDatabaseEntities1 db = new FinalDatabaseEntities1())
                {

                    if ((objUser.username.Equals("admin")) && (objUser.password.Equals("admin")))
                    {
                        Session["username"] = objUser.username.ToString();
                        Session["password"] = objUser.password.ToString();
                        return RedirectToAction("UserDashBoard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The username or password is incorrect");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}
