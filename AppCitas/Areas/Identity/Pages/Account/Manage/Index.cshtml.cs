using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppCitas.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombres")]
            public string Nombres { get; set; }
            [Required]
            [Display(Name = "Apellidos")]
            public string Apellidos { get; set; }

            [Required]
            [Display(Name = "RUN")]
            public string RUN { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var nombres = user.Nombres;
            var apellidos = user.Apellidos;
            var RUN = user.RUN;

            Username = userName;

            Input = new InputModel
            {
                Nombres = nombres,
                Apellidos = apellidos,
                RUN = RUN,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (User.Identity.Name != null)
            {
                var user2 = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user2.Administrador;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                }
            }

            if (userId != null)
            {
                TempData["userId"] = userId;
            }
            //else 
            //{
            //    TempData["userId"] = null;
            //}

            //TempData["habilitarCitas"] = "1";

            var user = await _userManager.GetUserAsync(User);

            if (TempData["userId"] != null)
            {
                user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            TempData["userId"] = user.Id;
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (userId != null)
            {
                TempData["userId"] = userId;
            }

            var user = await _userManager.GetUserAsync(User);

            if (TempData["userId"] != null)
            {
                user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            }

            var nombres = user.Nombres;
            var apellidos = user.Apellidos;
            var RUN = user.RUN;
            if (Input.Nombres != nombres)
            {
                user.Nombres = Input.Nombres;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Apellidos != apellidos)
            {
                user.Apellidos = Input.Apellidos;
                await _userManager.UpdateAsync(user);
            }
            if (Input.RUN != RUN)
            {
                user.RUN = Input.RUN;
                await _userManager.UpdateAsync(user);
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = " El perfil ha sido modificado";

            TempData["userId"] = user.Id;
            return RedirectToPage();
            //return Redirect("~/Usuario");
        }
    }
}
