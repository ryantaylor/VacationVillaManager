using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models
{
    public class Cost
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public double? Amount { get; set; }
        public House House { get; set; }
        public Booking Booking { get; set; }
        public Special Special { get; set; }
        public bool CalculateDaily { get; set; }

        public Cost()
        {
            CalculateDaily = false;
        }
    }
}