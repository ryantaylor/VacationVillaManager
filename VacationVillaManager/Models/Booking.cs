using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Client Client { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }

        public House House { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Rate { get; set; }
        public List<Cost> Costs { get; set; }

        public Booking()
        {
            Client = new Client();
            Costs = new List<Cost>();
        }
    }
}