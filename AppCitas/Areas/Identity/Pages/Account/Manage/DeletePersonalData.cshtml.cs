using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppCitas.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet(string userId)
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
            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (userId != null)
            {
                TempData["userId"] = userId;
            }

            var user = await _userManager.GetUserAsync(User);
            var user1 = await _userManager.GetUserAsync(User);

            if (TempData["userId"] != null)
            {
                user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            }

            //var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //RequirePassword = await _userManager.HasPasswordAsync(user);
            //if (RequirePassword)
            //{
            //    if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            //    {
            //        ModelState.AddModelError(string.Empty, "Incorrect password.");
            //        return Page();
            //    }
            //}

            var result = await _userManager.DeleteAsync(user);
            var userId1 = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user1.UserName}'.");
            }

            //await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId1);

            TempData["userId"] = user.Id;

            return Redirect("~/Usuario");
        }
    }
}
