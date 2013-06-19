using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VacationVillaManager.Models
{
    public enum ManagerType
    {
        OWNER = 0,
        COMPANY = 1
    }

    public class HouseManager
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public Location Location { get; set; }
        public ManagerType? Type { get; set; }

        public string ContactNumberA { get; set; }
        public string ContactNumberB { get; set; }

        public HouseManager()
        {
            Type = null;
        }

        public HouseManager(ManagerType type)
        {
            Type = type;
        }
    }
}