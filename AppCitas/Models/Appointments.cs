using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppCitas.Models
{
    
    public class Appointments
    {
        [Key]
        public long IdAppointments { get; set; }

        [Required]
        public string DescriptionApp { get; set; }
        [Required]
        public string ServiceType { get; set; }
        [Required]
        public string Establishment { get; set; }
        [Required]
        public string Professional { get; set; }
    }
}
