using System.Threading.Tasks;
using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppCitas.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
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
            return Page();
        }
    }
}