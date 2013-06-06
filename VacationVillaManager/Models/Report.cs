using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VacationVillaManager.Models
{
    public class Report
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Type { get; set; }

        public static IEnumerable<SelectListItem> BuildReportTypesDropdownList()
        {
            ManagerContext db = new ManagerContext();

            List<SelectListItem> typesList = new List<SelectListItem>();

            typesList.Add(new SelectListItem
            {
                Value = "0",
                Text = "Select One",
            });

            typesList.Add(new SelectListItem
            {
                Value = "1",
                Text = "Client Emails",
            });

            typesList.Add(new SelectListItem
            {
                Value = "2",
                Text = "Monthly Tax Report",
            });

            typesList.Add(new SelectListItem
            {
                Value = "3",
                Text = "Available Houses",
            });

            typesList.Add(new SelectListItem
            {
                Value = "4",
                Text = "Arrival/Departure Dates",
            });

            return typesList;
        }
    }
}