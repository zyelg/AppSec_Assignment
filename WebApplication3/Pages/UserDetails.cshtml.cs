using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WebApplication3.Model;
using Microsoft.AspNetCore.DataProtection;

namespace WebApplication3.Pages
{
    [Authorize]
    public class UserDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDetailsModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser ApplicationUser { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Decrypt the NRIC number
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");
            user.NRIC = protector.Unprotect(user.NRIC);

            ApplicationUser = user;
            return Page();
        }
    }
}
