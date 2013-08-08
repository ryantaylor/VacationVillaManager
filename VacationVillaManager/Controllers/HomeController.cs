using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;

namespace VacationVillaManager.Controllers
{
    public class HomeController : Controller
    {
        ManagerContext db = new ManagerContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            ViewBag.Headlines = db.Photos.Include("House").Where(m => m.IsHeadline == true).ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }

        public ActionResult Services()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }
    }
}
