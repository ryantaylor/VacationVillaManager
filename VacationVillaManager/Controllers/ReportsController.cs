using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;
using VacationVillaManager.Models.ReportModels;

namespace VacationVillaManager.Controllers
{
    public class ReportsController : Controller
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

        public ActionResult GenerateEmailReport(ReportEmailModel model)
        {
            return View();
        }

        //
        // POST: /Reports/GenerateTaxesReport

        public ActionResult GenerateTaxesReport(ReportTaxesModel model)
        {
            return View();
        }

        //
        // POST: /Reports/GenerateFreeHousesReport

        public ActionResult GenerateFreeHousesReport(ReportFreeHousesModel model)
        {
            return View();
        }

        //
        // POST: /Reports/GenerateFreeHousesReport

        public ActionResult GenerateComeGoReport(ReportComeGoModel model)
        {
            return View();
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
    }
}