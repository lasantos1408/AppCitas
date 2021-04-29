using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCitas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            //TempData["habilitarCitas"] = "1";

            var users = await _userManager.Users.ToListAsync();

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var administrador = user.Administrador;

            if (administrador == true)
            {
                TempData["hablitarUser"] = "1";
            }


            return View(users);
        }


    }
}
