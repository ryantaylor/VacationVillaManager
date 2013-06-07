using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models.ReportModels
{
    public class ReportTaxesModel
    {
        public List<int> Rates { get; set; }
        public DateTime Month { get; set; }

        public ReportTaxesModel()
        {
            Rates = new List<int>();
            for (int i = 0; i < 3; i++)
                Rates.Add(0);
        }
    }
}