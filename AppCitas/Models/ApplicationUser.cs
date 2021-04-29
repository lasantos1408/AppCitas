using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCitas.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RUN { get; set; }
        public bool Administrador { get; set; }
    }
}
