using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models.ReportModels
{
    public class ReportComeGoModel
    {
        public DateTime Month { get; set; }
        public string Email { get; set; }
        public Dictionary<string, List<Booking>> HouseBookings { get; set; }
    }
}