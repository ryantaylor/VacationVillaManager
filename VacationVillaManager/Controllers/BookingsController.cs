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
    public class BookingsController : Controller
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Bookings/

        public ActionResult Index()
        {
            return View(db.Bookings.ToList());
        }

        //
        // GET: /Bookings/Details/5

        public ActionResult Details(int id = 0)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //
        // GET: /Bookings/Create

        public ActionResult Create()
        {
            ViewData["ClientsList"] = Client.BuildClientsDropdownList();
            ViewData["HousesList"] = House.BuildHousesDropdownList();
            return View(new Booking());
        }

        //
        // POST: /Bookings/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            ViewData["ClientsList"] = Client.BuildClientsDropdownList();
            ViewData["HousesList"] = House.BuildHousesDropdownList();

            if (ModelState.IsValid && House.IsAvailableInRange(booking.StartDate, booking.EndDate, booking.House.ID))
            {
                booking.House = db.Houses.Include("Location").Single(m => m.ID == booking.House.ID);
                List<Cost> costs = new List<Cost>();

                foreach (Cost c in booking.Costs)
                {
                    if (c.Name != null)
                        costs.Add(c);
                }

                booking.Costs = costs;

                if (booking.Client.ID != 0)
                {
                    Client client = booking.Client;
                    db.Entry(client).State = EntityState.Modified;
                    booking.Client = db.Clients.Include("Location").Single(m => m.ID == client.ID);
                }

                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        //
        // PARTIAL: /Bookings/ClientEditor/5

        public PartialViewResult _ClientEditor(int id = 0)
        {
            ViewBag.NewClient = true;
            if (id == 0)
                return PartialView();

            Booking booking = new Booking();
            booking.Client = db.Clients.Include("Location").Single(m => m.ID == id);
            ViewBag.NewClient = false;

            return PartialView(booking);
        }

        //
        // PARTIAL: /Bookings/CostEditor/5

        public PartialViewResult _CostEditor(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // PARTIAL: /Bookings/CostEditorWithID/5

        public PartialViewResult _CostEditorWithID(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // AJAX: /Bookings/GetRate/5

        public double GetRate(int id = 0)
        {
            if (id > 0)
            {
                return db.Houses.Single(m => m.ID == id).Rate;
            }

            return 0;
        }

        //
        // AJAX: /Bookings/GetCosts/5

        public ActionResult GetCosts(int id = 0)
        {
            List<Cost> costs = db.Costs.Where(m => m.House.ID == id).ToList();
            return Json(new { Costs = costs }, JsonRequestBehavior.AllowGet);
        }

        //
        // AJAX: /Bookings/GetBookingCosts/5

        public ActionResult GetBookingCosts(int id = 0)
        {
            List<Cost> costs = db.Costs.Where(m => m.Booking.ID == id).ToList();
            return Json(new { Costs = costs }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Bookings/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Booking booking = db.Bookings.Include("Client")
                                         .Include("Client.Location")
                                         .Include("House")
                                         .Include("House.Location")
                                         .Single(m => m.ID == id);
            ViewData["HousesList"] = House.BuildHousesDropdownList();
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //
        // POST: /Bookings/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                //House temp = db.Houses.Include("Location").Single(m => m.ID == booking.House.ID);
                //db.Entry(temp).CurrentValues.SetValues(booking.House);
                //db.Entry(temp).CurrentValues.SetValues(booking.House);
                db.Entry(booking.House).State = EntityState.Modified;
                //db.Entry(booking).CurrentValues.SetValues(booking);
                db.Entry(booking.Client).State = EntityState.Modified;
                db.Entry(booking.Client.Location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        //
        // GET: /Bookings/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //
        // POST: /Bookings/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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