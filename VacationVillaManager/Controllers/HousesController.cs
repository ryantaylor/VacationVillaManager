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
    public class HousesController : Controller
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /House/

        public ActionResult Index()
        {
            return View(db.Houses.ToList());
        }

        //
        // GET: /House/Details/5

        public ActionResult Details(int id = 0)
        {
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        //
        // GET: /House/Create

        public ActionResult Create()
        {
            return View(new House());
        }

        //
        // POST: /House/Create

        [HttpPost]
        public ActionResult Create(House house)
        {
            if (ModelState.IsValid)
            {
                foreach (Cost c in house.Costs)
                {
                    if (c.Name.Equals(null))
                        house.Costs.Remove(c);
                }

                db.Houses.Add(house);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(house);
        }

        //
        // PARTIAL: /Houses/CostEditor

        public PartialViewResult CostEditor(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // GET: /House/Edit/5

        public ActionResult Edit(int id = 0)
        {
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        //
        // POST: /House/Edit/5

        [HttpPost]
        public ActionResult Edit(House house)
        {
            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(house);
        }

        //
        // GET: /House/Delete/5

        public ActionResult Delete(int id = 0)
        {
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        //
        // POST: /House/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            House house = db.Houses.Find(id);
            db.Houses.Remove(house);
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