using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VacationVillaManager.Models;

namespace VacationVillaManager.Controllers
{
    
    public class HousesController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /House/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Houses.Include("Location").Include("Owner").ToList());
        }

        //
        // GET: /House/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);

            House house = db.Houses.Include("Location").Single(m => m.ID == id);
            ViewBag.HouseID = id;
            ViewData["Bookings"] = db.Bookings.Where(m => m.House.ID == id)
                                                .Select(m => new
                                                {
                                                    StartDate = m.StartDate,
                                                    EndDate = m.EndDate
                                                });
            List<Special> specials = db.Specials.Where(m => m.House.ID == id && m.EndDate > DateTime.Now).OrderBy(m => m.StartDate).ToList();
            List<Cost> specialsCosts = new List<Cost>();
            foreach (Special s in specials)
            {
                specialsCosts.AddRange(db.Costs.Where(m => m.Special.ID == s.ID).ToList());
            }

            ViewData["Specials"] = specials;
            ViewData["SpecialsCosts"] = specialsCosts;

            house.Photos = db.Photos.Where(m => m.House.ID == id).ToList();
            house.Costs = db.Costs.Where(m => m.House.ID == id).ToList();

            if (house == null)
            {
                return HttpNotFound();
            }

            return View(house);
        }

        //
        // GET: /House/Availability

        public ActionResult Availability()
        {
            if (Session["ActiveHouses"] == null) Session["ActiveHouses"] = db.Houses.Where(m => m.Active == true);

            return View(new AvailabilityModel());
        }

        //
        // POST: /House/Availability

        [HttpPost]
        public PartialViewResult Availability(AvailabilityModel model)
        {
            List<House> activeHouses = db.Houses.Where(m => m.Active == true).ToList();
            List<House> freeHouses = new List<House>();
            foreach (House h in activeHouses)
            {
                if (House.IsAvailableInRange(model.StartDate, model.EndDate, h.ID))
                {
                    h.Photos = db.Photos.Where(m => m.House.ID == h.ID && m.IsHeadline == true).ToList();
                    freeHouses.Add(h);
                }
            }
            return PartialView("_AvailabilityResults", freeHouses);
        }

        //
        // GET: /House/Create

        [Authorize]
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

                foreach (Photo p in house.Photos)
                {
                    string temp = Request.PhysicalApplicationPath + "/Images/temp/";
                    string dest = Request.PhysicalApplicationPath + "/Images/houses/";
                    System.IO.File.Move(temp + p.URL, dest + p.URL);
                    System.IO.File.Move(temp + p.URLThumb, dest + p.URLThumb);
                    System.IO.File.Move(temp + p.URLThumbWide, dest + p.URLThumbWide);
                }

                Success(house.Name + " was successfully created!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong!");
            return View(house);
        }

        //
        // PARTIAL: /Houses/CostEditor

        [Authorize]
        public PartialViewResult _CostEditor(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // PARTIAL: /Houses/CostEditorWithID

        [Authorize]
        public PartialViewResult _CostEditorWithID(int id = 0)
        {
            ViewBag.CostID = id;
            return PartialView();
        }

        //
        // GET: /House/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            House house = db.Houses.Include("Location").Include("Owner").Include("Owner.Location").Include("ManagementCompany").Include("ManagementCompany.Location").Single(m => m.ID == id);
            house.Photos = db.Photos.Where(m => m.House.ID == house.ID).ToList();
            house.Costs = db.Costs.Where(m => m.House.ID == house.ID).ToList();
            foreach (Cost c in house.Costs)
                c.House = null;
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
                //House h = db.Houses.Include("Location").Include("Owner").Include("Owner.Location").Include("ManagementCompany").Include("ManagementCompany.Location").Single(m => m.ID == house.ID);
                /*h.Name = house.Name;
                h.SecurityCode = house.SecurityCode;
                h.PhoneNumber = house.PhoneNumber;
                h.Rate = house.Rate;
                h.Location.Address = house.Location.Address;*/
                House h = db.Houses.Include("Location").Include("Owner").Include("Owner.Location").Include("ManagementCompany").Include("ManagementCompany.Location").Single(m => m.ID == house.ID);
                db.Entry(h).CurrentValues.SetValues(house);
                db.Entry(h.Location).CurrentValues.SetValues(house.Location);
                db.Entry(h.Owner).CurrentValues.SetValues(house.Owner);
                db.Entry(h.Owner.Location).CurrentValues.SetValues(house.Owner.Location);
                db.Entry(h.ManagementCompany).CurrentValues.SetValues(house.ManagementCompany);
                db.Entry(h.ManagementCompany.Location).CurrentValues.SetValues(house.ManagementCompany.Location);
                //db.Entry(house).State = EntityState.Modified;
                //db.Entry(house.Location).State = EntityState.Modified;
                //db.Entry(house.Owner).State = EntityState.Modified;
                //db.Entry(house.ManagementCompany).State = EntityState.Modified;

                List<Cost> newCosts = house.Costs;

                for (int i = 0; i < newCosts.Count; i ++)
                {
                    Cost c = newCosts.ElementAt(i);
                    if (c.ID > 0)
                    {
                        if (c.Name == null)
                        {
                            db.Entry(c).State = EntityState.Deleted;
                            //i--;
                        }
                        else
                            db.Entry(c).State = EntityState.Modified;
                    }
                    else
                    {
                        if (c.Name != null)
                        {
                            c.House = h;
                            db.Entry(c).State = EntityState.Added;
                        }
                            //db.Costs.Add(c);
                    }
                }

                List<Photo> newPhotos = house.Photos;

                for (int i = 0; i < newPhotos.Count; i++)
                {
                    Photo p = newPhotos.ElementAt(i);
                    if (p.ID > 0)
                    {
                        if (p.URL == null)
                        {
                            string path = Request.PhysicalApplicationPath + "Images\\houses\\";
                            p.URL = p.URLThumb.Replace(".thumb", "");
                            System.IO.File.Delete(path + p.URL);
                            System.IO.File.Delete(path + p.URLThumb);
                            System.IO.File.Delete(path + p.URLThumbWide);
                            db.Entry(p).State = EntityState.Deleted;
                        }
                        else
                            db.Entry(p).State = EntityState.Modified;
                    }
                    else
                    {
                        if (p.URL != null)
                        {
                            p.House = h;
                            db.Entry(p).State = EntityState.Added;

                            string temp = Request.PhysicalApplicationPath + "Images\\temp\\";
                            string dest = Request.PhysicalApplicationPath + "Images\\houses\\";
                            System.IO.File.Move(temp + p.URL, dest + p.URL);
                            System.IO.File.Move(temp + p.URLThumb, dest + p.URLThumb);
                            System.IO.File.Move(temp + p.URLThumbWide, dest + p.URLThumbWide);
                        }
                    }

                }

                db.SaveChanges();
                Success("Changes were successfully saved!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong! Changes were not saved.");
            return View(house);
        }

        //
        // GET: /House/Delete/5

        [Authorize]
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
        // GET: /House/ToggleActive/5

        [Authorize]
        public ActionResult ToggleActive(int id)
        {
            House house = db.Houses.Include("Location").Include("Owner").Include("ManagementCompany").Single(m => m.ID == id);
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

        /*protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }*/
    }
}