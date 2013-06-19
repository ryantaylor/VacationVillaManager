using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VacationVillaManager.Models
{
    public class House
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public Location Location { get; set; }

        public string PhoneNumber { get; set; }
        public string SecurityCode { get; set; }

        public double Rate { get; set; }
        public string VirtualTour { get; set; }
        public string Flyer { get; set; }
        public string Description { get; set; }

        public string HomeAwayID { get; set; }

        public List<Cost> Costs { get; set; }
        public List<Photo> Photos { get; set; }

        public HouseManager Owner { get; set; }
        public HouseManager ManagementCompany { get; set; }

        public Boolean Active { get; set; }

        public House()
        {
            Costs = new List<Cost>();
            Photos = new List<Photo>();
            Location = new Location();
            Owner = new HouseManager(ManagerType.OWNER);
            ManagementCompany = new HouseManager(ManagerType.COMPANY);
        }

        public static IEnumerable<SelectListItem> BuildHousesDropdownList()
        {
            ManagerContext db = new ManagerContext();

            List<SelectListItem> housesList = new List<SelectListItem>();
            List<House> houses = db.Houses.ToList();

            housesList.Add(new SelectListItem
            {
                Value = "",
                Text = "Select One",
            });

            foreach (House h in houses)
            {
                housesList.Add(new SelectListItem
                {
                    Value = h.ID.ToString(),
                    Text = h.Name,
                });
            }

            return housesList;
        }

        public static Boolean IsAvailableInRange(DateTime start, DateTime end, int houseID)
        {
            if (start > end)
                return false;

            ManagerContext db = new ManagerContext();

            List<Booking> bookings = db.Bookings.Where(m => m.House.ID == houseID).ToList();
            foreach (Booking b in bookings)
            {
                if ((start >= b.StartDate && start < b.EndDate) || (end > b.StartDate && end <= b.EndDate) || (start < b.StartDate && end > b.EndDate))
                    return false;
            }
            return true;
        }
    }
}