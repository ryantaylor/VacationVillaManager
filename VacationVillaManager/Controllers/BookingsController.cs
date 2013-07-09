using CloudinaryDotNet;
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
    [Authorize]
    public class BookingsController : BootstrapBaseController
    {
        private ManagerContext db = new ManagerContext();

        //
        // GET: /Bookings/

        public ActionResult Index()
        {
            return View(db.Bookings.Include("House").Include("Client").OrderByDescending(m => m.StartDate).ToList());
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
                booking.House = db.Houses.Include("Location").Include("Owner").Include("Owner.Location").Include("ManagementCompany").Include("ManagementCompany.Location").Single(m => m.ID == booking.House.ID);
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
                Success("The booking was created successfully!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong!");
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
                //db.Entry(temp).CurrentValues.SetValues(booking.House);//db.Entry(booking.House).State = EntityState.Modified;
                //db.Entry(booking).CurrentValues.SetValues(booking);

                //db.Entry(booking).State = EntityState.Modified;
                //db.Entry(booking.Client).State = EntityState.Modified;
                //db.Entry(booking.Client.Location).State = EntityState.Modified;

                Booking b = db.Bookings.Include("House").Include("House.Location").Include("Client").Include("Client.Location").Single(m => m.ID == booking.ID);
                db.Entry(b).CurrentValues.SetValues(booking);
                db.Entry(b.Client).CurrentValues.SetValues(booking.Client);
                db.Entry(b.Client.Location).CurrentValues.SetValues(booking.Client.Location);

                List<Cost> newCosts = booking.Costs;

                for (int i = 0; i < newCosts.Count; i++)
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
                            //c.Booking = new Booking();
                            //c.Booking.ID = booking.ID;
                            c.Booking = b;
                            db.Entry(c).State = EntityState.Added;
                        }
                    }
                }

                /*for (int i = 0; i < booking.Costs.Count; i++)
                {
                    //Cost c = newCosts.ElementAt(i);
                    if (booking.Costs[i].ID > 0)
                    {
                        if (booking.Costs[i].Name == null)
                            //db.Costs.Remove(c);
                            db.Entry(booking.Costs[i]).State = EntityState.Deleted;
                        else
                            db.Entry(booking.Costs[i]).State = EntityState.Modified;
                    }
                    else
                    {
                        if (booking.Costs[i].Name != null)
                        {
                            //booking.Costs[i].Booking = booking;
                            db.Entry(booking.Costs[i]).State = EntityState.Added;
                        }
                    }
                }*/

                db.SaveChanges();
                Success("Changes were successfully saved!");
                return RedirectToAction("Index");
            }
            Error("Something went wrong! Changes were not saved.");
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

        //
        // GET: /Bookings/PhotoUpload

        public ActionResult PhotoUpload()
        {
            return View();
        }

        //
        // POST: /Bookings/PhotoUpload

        [HttpPost]
        public ActionResult PhotoUpload(string qqfile, string houseID)
        {
            // change based on image hosting solution
            Account account = new Account("hmnrrn3zr", "422391321397551", "aKXsVxkoax0wxFp71NU_m8QtHBk");
            Cloudinary cloudinary = new Cloudinary(account);

            var path = "/Data/Applications/Cygwin/home/Ryan/VacationVillaManager/VacationVillaManager/App_Data/uploads/";
            var file = string.Empty;

            try
            {
                var stream = Request.InputStream;
                if (String.IsNullOrEmpty(Request["qqfile"]))
                {
                    // IE
                    HttpPostedFileBase postedFile = Request.Files[0];
                    stream = postedFile.InputStream;
                    file = Path.Combine(path, System.IO.Path.GetFileName(Request.Files[0].FileName));
                }
                else
                {
                    //Webkit, Mozilla
                    file = Path.Combine(path, qqfile);
                }

                //var buffer = new byte[stream.Length];
                //stream.Read(buffer, 0, buffer.Length);
                CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new CloudinaryDotNet.Actions.FileDescription(qqfile, stream)
                };

                CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

                string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                string thumb = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(200).Height(200).Crop("thumb")).BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));

                return Json(new { success = true, path = url, thumb = thumb, name = qqfile }, "text/html");

                //System.IO.File.WriteAllBytes(file, buffer);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, "application/json");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}