using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models
{
    public class House
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ZipPostalCode { get; set; }

        public double Rate { get; set; }
        public string VirtualTour { get; set; }
        public string Flyer { get; set; }
        public string Description { get; set; }

        public string HomeAwayID { get; set; }

        public List<string> Photos { get; set; }
        public List<Cost> Costs
        {
            get
            {
                ManagerContext db = new ManagerContext();
                return db.Costs.Where(r => r.HouseID == ID).ToList();
            }
        }
    }
}