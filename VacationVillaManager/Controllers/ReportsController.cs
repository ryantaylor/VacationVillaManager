using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;
using VacationVillaManager.Models.ReportModels;

namespace VacationVillaManager.Controllers
{
    [Authorize]
    public class ReportsController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Reports/

        public ActionResult Index()
        {
            return View(db.Reports.ToList());
        }

        //
        // GET: /Reports/Details/5

        public ActionResult Details(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // GET: /Reports/Create

        public ActionResult Create()
        {
            ViewData["TypesList"] = Report.BuildReportTypesDropdownList();
            return View();
        }

        //
        // POST: /Reports/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        //
        // AJAX: /Reports/GenerateReportEditor/5

        public PartialViewResult GenerateReportEditor(int id = 0)
        {
            switch (id)
            {
                case 1:
                    return PartialView("_ReportEmailEditor", new ReportEmailModel());

                case 2:
                    return PartialView("_ReportTaxesEditor", new ReportTaxesModel());

                case 3:
                    return PartialView("_ReportFreeHousesEditor", new ReportFreeHousesModel());

                case 4:
                    return PartialView("_ReportComeGoEditor", new ReportComeGoModel());

                default:
                    break;
            }

            return PartialView();
        }

        //
        // POST: /Reports/GenerateEmailReport

        public PartialViewResult GenerateEmailReport()
        {
            ReportEmailModel model = new ReportEmailModel();
            model.Clients = db.Clients.Where(m => m.Email != null);
            return PartialView("_ViewEmail", model);
        }

        //
        // POST: /Reports/GenerateTaxesReport

        public PartialViewResult GenerateTaxesReport(ReportTaxesModel model)
        {
            List<TaxViewModel> taxes = new List<TaxViewModel>();
            List<House> houses = db.Houses.ToList();

            foreach (House h in houses)
            {
                if (h.Name != null)
                {
                    List<Booking> bookings = db.Bookings.Where(m => m.House.ID == h.ID && ((m.StartDate.Month == model.Month.Month && m.StartDate.Year == model.Month.Year) || (m.EndDate.Month == model.Month.Month && m.EndDate.Year == model.Month.Year))).ToList();
                    TaxViewModel taxModel = new TaxViewModel();
                    taxModel.House = h;
                    taxModel.Month = model.Month;

                    foreach (Booking b in bookings)
                    {
                        taxModel.NumBookings++;
                        if (b.PaidInFull) taxModel.NumPaidFull++;
                        if (b.StartDate.Month == b.EndDate.Month)
                        {
                            taxModel.Subtotal += b.Subtotal;
                            TimeSpan span = b.EndDate - b.StartDate;
                            taxModel.NumNights += (int)Math.Round(span.TotalDays);
                        }
                        else
                        {
                            int taxableNights = 0;
                            int totalNights = 0;
                            DateTime start = b.StartDate;
                            DateTime end = b.EndDate;
                            while (!isSameDay(start, end))
                            {
                                if (start.Month == model.Month.Month) taxableNights++;
                                start = start.AddDays(1);
                                totalNights++;
                            }
                            taxModel.Subtotal += b.Subtotal * ((double)taxableNights / totalNights);
                            taxModel.NumNights += taxableNights;
                        }
                    }

                    foreach (double rate in model.Rates)
                    {
                        taxModel.Taxes.Add(rate, taxModel.Subtotal * (rate / 100));
                    }

                    taxes.Add(taxModel);
                }
                
            }
            return PartialView("_ViewTaxes", taxes);
        }

        //
        // POST: /Reports/GenerateFreeHousesReport

        public PartialViewResult GenerateFreeHousesReport(ReportFreeHousesModel model)
        {
            List<House> allHouses = db.Houses.ToList();
            List<House> freeHouses = new List<House>();

            foreach (House h in allHouses)
            {
                if (House.IsAvailableInRange(model.StartDate, model.EndDate, h.ID))
                    freeHouses.Add(h);
            }
            model.Houses = freeHouses;
            return PartialView("_ViewFreeHouses", model);
        }

        //
        // POST: /Reports/GenerateComeGoReport

        public PartialViewResult GenerateComeGoReport(ReportComeGoModel model)
        {
            List<Booking> relevantBookings = db.Bookings.Include("Client").Where(m => m.StartDate.Month == model.Month.Month || m.EndDate.Month == model.Month.Month).ToList();
            List<House> houses = db.Houses.ToList();

            model.HouseBookings = new Dictionary<string, List<Booking>>();

            foreach (House h in houses)
            {
                List<Booking> connectedBookings = new List<Booking>();

                /*mail.Body += "<table>" +
                                "<tr>" +
                                    "<th style='width: 33%;'>Client</td>" +
                                    "<th style='width: 33%;'>Arrival</td>" +
                                    "<th style='width: 33%;'>Departure</td>" +
                                "</tr>";*/

                foreach (Booking b in relevantBookings)
                {
                    if (h.ID == b.House.ID)
                    {
                        connectedBookings.Add(b);
                    }
                }

                if (h.Name != null)
                    model.HouseBookings.Add(h.Name, connectedBookings);
            }

            if (model.Email != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.lin.arvixe.com");
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@vacationvillamanager.com", "webmastervvm");

                mail.From = new MailAddress("info@vacationvillamanager.com");
                mail.To.Add(model.Email);
                mail.Subject = "Arrivals/Departures Report - " + model.Month.ToString("MMMM yyyy");
                mail.IsBodyHtml = true;

                mail.Body = RenderViewToString("_ViewComeGo", model);

                SmtpServer.Send(mail);
                mail.Dispose();
            }

            return PartialView("_ViewComeGo", model);
        }

        private bool isSameDay(DateTime first, DateTime second)
        {
            return (first.Day == second.Day && first.Month == second.Month && first.Year == second.Year);
        }

        //
        // GET: /Reports/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // POST: /Reports/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        //
        // GET: /Reports/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // POST: /Reports/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}