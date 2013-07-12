using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models.ReportModels
{
    public class TaxViewModel
    {
        public House House { get; set; }
        public DateTime Month { get; set; }
        public int NumBookings { get; set; }
        public int NumPaidFull { get; set; }
        public int NumNights { get; set; }
        public double Subtotal { get; set; }
        public Dictionary<double, double> Taxes { get; set; }

        public TaxViewModel()
        {
            Taxes = new Dictionary<double, double>();
        }
    }
}