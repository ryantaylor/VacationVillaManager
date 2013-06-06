using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;

namespace VacationVillaManager.Controllers
{
    public class InquiriesController : Controller
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Inquiries/

        public ActionResult Index()
        {
            return View(db.Inquiries.ToList());
        }

        //
        // GET: /Inquiries/Details/5

        public ActionResult Details(int id = 0)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        //
        // GET: /Inquiries/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Inquiries/Create

        [HttpPost]
        public ActionResult Create(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.House = db.Houses.Include("Location").Single(m => m.ID == inquiry.House.ID);
                inquiry.Status = "Pending";
                db.Inquiries.Add(inquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inquiry);
        }

        //
        // GET: /Inquiries/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        //
        // POST: /Inquiries/Edit/5

        [HttpPost]
        public ActionResult Edit(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inquiry);
        }

        //
        // GET: /Inquiries/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        //
        // POST: /Inquiries/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            db.Inquiries.Remove(inquiry);
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