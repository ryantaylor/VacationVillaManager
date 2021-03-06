﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;

namespace VacationVillaManager.Controllers
{
    public class InquiriesController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Inquiries/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Inquiries.Include("House").ToList());
        }

        //
        // GET: /Inquiries/Details/5

        [Authorize]
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

        /*public ActionResult Create()
        {
            return View();
        }*/

        //
        // POST: /Inquiries/Create

        [HttpPost]
        public bool Create(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.House = db.Houses.Include("Location").Single(m => m.ID == inquiry.House.ID);
                inquiry.Status = "Pending";
                db.Inquiries.Add(inquiry);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        //
        // GET: /Inquiries/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Inquiry inquiry = db.Inquiries.Include("House").Single(m => m.ID == id);
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
                Inquiry i = db.Inquiries.Include("House").Single(m => m.ID == inquiry.ID);
                db.Entry(i).CurrentValues.SetValues(inquiry);
                db.SaveChanges();
                Success("Response sent!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong! No response was sent");
            return View(inquiry);
        }

        //
        // GET: /Inquiries/Delete/5

        [Authorize]
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