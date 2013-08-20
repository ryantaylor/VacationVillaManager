using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models
{
    public class GuestReview
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public House House { get; set; }

        public string GuestName { get; set; }
        public string Excerpt { get; set; }
        public string ReviewFull { get; set; }
    }
}