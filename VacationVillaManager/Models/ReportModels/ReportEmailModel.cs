using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models.ReportModels
{
    public class ReportEmailModel
    {
        public int NumberOfEmails { get; set; }
        public IEnumerable<Client> Clients { get; set; }

        public ReportEmailModel()
        {
            ManagerContext db = new ManagerContext();
            NumberOfEmails = db.Clients.Count(m => m.Email != null);
        }
    }
}