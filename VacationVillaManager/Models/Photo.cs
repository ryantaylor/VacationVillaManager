using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationVillaManager.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public string URL { get; set; }
        public string URLThumb { get; set; }
        public string URLThumbWide { get; set; }
        public House House { get; set; }
        public bool IsHeadline { get; set; }
        public int Order { get; set; }
    }
}