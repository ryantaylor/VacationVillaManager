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
    public class SpecialsController : Controller
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Specials/

        public ActionResult Index()
        {
            return View(db.Specials.ToList());
        }

        //
        // GET: /Specials/Details/5

        public ActionResult Details(int id = 0)
        {
            Special special = db.Specials.Find(id);
            if (special == null)
            {
                return HttpNotFound();
            }
            return View(special);
        }

        //
        // GET: /Specials/Create

        public ActionResult Create()
        {
            ViewData["HousesList"] = House.BuildHousesDropdownList();
            return View(new Special());
        }

        //
        // POST: /Specials/Create

        [HttpPost]
        public ActionResult Create(Special special)
        {
            if (ModelState.IsValid)
            {
                special.House = db.Houses.Include("Location").Single(m => m.ID == special.House.ID);
                db.Specials.Add(special);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(special);
        }

        //
        // GET: /Specials/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Special special = db.Specials.Include("House").Single(m => m.ID == id);
            if (special == null)
            {
                return HttpNotFound();
            }
            return View(special);
        }

        //
        // POST: /Specials/Edit/5

        [HttpPost]
        public ActionResult Edit(Special special)
        {
            if (ModelState.IsValid)
            {
                db.Entry(special).State = EntityState.Modified;
                foreach (Cost c in special.Costs)
                    db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(special);
        }

        //
        // AJAX: /Specials/GetCosts/5

        public ActionResult GetCosts(int id = 0)
        {
            List<Cost> costs = db.Costs.Where(m => m.Special.ID == id).ToList();
            return Json(new { Costs = costs }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Specials/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Special special = db.Specials.Find(id);
            if (special == null)
            {
                return HttpNotFound();
            }
            return View(special);
        }

        //
        // POST: /Specials/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Special special = db.Specials.Find(id);
            db.Specials.Remove(special);
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