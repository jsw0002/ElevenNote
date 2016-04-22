using ElevenNote.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            try
            {
                var FirstName = currentUser.FirstName;
                ViewBag.FirstName = FirstName;
            }
            catch (NullReferenceException FirstName)
            {
                FirstName = null;
                ViewBag.FirstName = FirstName;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }
    }
}