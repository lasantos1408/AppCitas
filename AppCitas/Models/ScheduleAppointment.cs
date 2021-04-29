using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppCitas.Models
{
    public class ScheduleAppointment
    {
        [Key]
        public long IdScheduleAppointment { get; set; }
        [Required]
        public string Performance { get; set; }
        [Required]
        public DateTime DateHourAppointment { get; set; }
        [Required]
        public DateTime DateCreation { get; set; }
        [Required]
        public string RUN { get; set; }
        [Required]
        public string PatientNames { get; set; }
        [Required]
        public long IdAppointments { get; set; }
        [Required]
        public string StatusAppointment { get; set; }
        [Required]
        public string Reference { get; set; }
    }
}
