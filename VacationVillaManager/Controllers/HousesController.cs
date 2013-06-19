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
    public class HousesController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /House/

        public ActionResult Index()
        {
            return View(db.Houses.Include("Location").Include("Owner").ToList());
        }

        //
        // GET: /House/Details/5

        public ActionResult Details(int id = 0)
        {
            House house = db.Houses.Find(id);
            ViewBag.HouseID = id;
            ViewData["HousesList"] = House.BuildHousesDropdownList();
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
                List<Cost> costs = new List<Cost>();

                foreach (Cost c in house.Costs)
                {
                    if (c.Name != null)
                        costs.Add(c);
                }

                List<Photo> photos = new List<Photo>();

                foreach (Photo p in house.Photos)
                {
                    if (p.URL != null)
                        photos.Add(p);
                }

                house.Costs = costs;
                house.Photos = photos;

                db.Houses.Add(house);
                db.SaveChanges();

                this.Success("Added!");
                return RedirectToAction("Index");
            }

            return View(house);
        }

        //
        // PARTIAL: /Houses/CostEditor

        public PartialViewResult _CostEditor(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // PARTIAL: /Houses/CostEditorWithID

        public PartialViewResult _CostEditorWithID(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // GET: /House/Edit/5

        public ActionResult Edit(int id = 0)
        {
            House house = db.Houses.Include("Location").Single(m => m.ID == id);
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
                db.Entry(house.Location).State = EntityState.Modified;

                List<Cost> newCosts = house.Costs;

                for (int i = 0; i < newCosts.Count; i ++)
                {
                    Cost c = newCosts.ElementAt(i);
                    if (c.ID > 0)
                    {
                        if (c.Name == null)
                        {
                            db.Entry(c).State = EntityState.Deleted;
                            i--;
                        }
                        else
                            db.Entry(c).State = EntityState.Modified;
                    }
                    else
                    {
                        if (c.Name != null)
                            db.Entry(c).State = EntityState.Added;
                    }
                }

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

        //
        // AJAX: /House/ToggleActive/5

        public ActionResult ToggleActive(int id)
        {
            House house = db.Houses.Single(m => m.ID == id);
            if (house.Active)
            {
                house.Active = false;
                this.Success(house.Name + " was successfully deactivated!");
            }

            else
            {
                house.Active = true;
                this.Success(house.Name + " was successfully activated!");
            }

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