using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            ViewBag.Headlines = db.Photos.Include("House").Where(m => m.IsHeadline == true).ToList();
            return View();
        }

        /*public ActionResult About()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }*/

        /*public ActionResult Contact()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }*/

        public ActionResult Services()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }

        public ActionResult Specials()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            List<Special> specials = db.Specials.Where(m => m.EndDate > DateTime.Now).OrderBy(m => m.StartDate).ToList();
            foreach (Special s in specials)
            {
                s.Costs = db.Costs.Where(m => m.Special.ID == s.ID).ToList();
            }

            List<House> houses = db.Houses.Where(m => m.Active == true).ToList();
            foreach (House h in houses)
            {
                h.Costs = db.Costs.Where(m => m.House.ID == h.ID).ToList();
            }
            ViewData["Houses"] = houses;

            return View(specials);
        }

        public ActionResult Attractions()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);
            return View();
        }

        public bool SendContact(ContactModel model)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mail.lin.arvixe.com");
            SmtpServer.Credentials = new System.Net.NetworkCredential("info@vacationvillamanager.com", "webmastervvm");

            mail.From = new MailAddress("info@vacationvillamanager.com");
            mail.To.Add("ryan.taylor9@gmail.com");
            mail.Subject = "VVM - Message from " + model.Email;
            mail.Body = "Name: " + model.Name + "<br />" +
                        "Email: " + model.Email + "<br />" +
                        "Message: " + model.Message;
            mail.IsBodyHtml = true;

            SmtpServer.Send(mail);
            mail.Dispose();
            return true;
        }

        public bool SendNewsletter(NewsletterModel model)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mail.lin.arvixe.com");
            SmtpServer.Credentials = new System.Net.NetworkCredential("info@vacationvillamanager.com", "webmastervvm");

            mail.From = new MailAddress("info@vacationvillamanager.com");
            mail.To.Add("ryan.taylor9@gmail.com");
            mail.Subject = "Newsletter subscription - " + model.Email;
            mail.Body = model.Email + " has asked to receive the newsletter.";
            mail.IsBodyHtml = true;

            SmtpServer.Send(mail);
            mail.Dispose();
            return true;
        }
    }
}
