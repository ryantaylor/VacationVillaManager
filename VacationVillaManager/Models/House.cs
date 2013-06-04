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

        public double Rate { get; set; }
        public string VirtualTour { get; set; }
        public string Flyer { get; set; }
        public string Description { get; set; }

        public string HomeAwayID { get; set; }

        public List<Cost> Costs { get; set; }
        public List<Photo> Photos { get; set; }

        public Boolean Active { get; set; }

        public House()
        {
            Costs = new List<Cost>();
            Photos = new List<Photo>();
            Location = new Location();
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
    }
}