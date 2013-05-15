using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VacationVillaManager.Models
{
    public class Client
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Location Location { get; set; }

        public Client()
        {
            Location = new Location();
        }

        public static IEnumerable<SelectListItem> BuildClientsDropdownList()
        {
            ManagerContext db = new ManagerContext();

            List<SelectListItem> clientsList = new List<SelectListItem>();
            List<Client> clients = db.Clients.ToList();

            clientsList.Add(new SelectListItem
            {
                Value = "",
                Text = "Select One",
            });

            foreach (Client c in clients)
            {
                clientsList.Add(new SelectListItem
                {
                    Value = c.ID.ToString(),
                    Text = c.Name + " - " + c.Email,
                });
            }

            return clientsList;
        }
    }
}