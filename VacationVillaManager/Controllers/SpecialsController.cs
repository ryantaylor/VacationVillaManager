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
    [Authorize]
    public class SpecialsController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Specials/

        public ActionResult Index()
        {
            return View(db.Specials.Include("House").OrderBy(m => m.StartDate).ToList());
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
            ViewData["HousesList"] = House.BuildHousesDropdownList();
            if (ModelState.IsValid)
            {
                special.House = db.Houses.Include("Location").Include("Owner").Include("Owner.Location").Include("ManagementCompany").Include("ManagementCompany.Location").Single(m => m.ID == special.House.ID);
                List<Cost> costs = new List<Cost>();

                foreach (Cost c in special.Costs)
                {
                    if (c.Name != null)
                        costs.Add(c);
                }

                special.Costs = costs;
                db.Specials.Add(special);
                Success("A special for " + special.House.Name + " was added successfully!");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Error("Something went wrong! The special was not added.");
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
                Success("Changes were successfully saved!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong! Changes were not saved.");
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