using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
	public class LoginModel : PageModel
	{
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IHttpContextAccessor httpContextAccessor;

		public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
		{
			this.signInManager = signInManager;
			this.httpContextAccessor = httpContextAccessor;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (!ValidateCaptcha())
				{
					ModelState.AddModelError("LModel.Email", "Please validate the CAPTCHA to prove that you are a human.");
					return Page();
				}

				var user = await signInManager.UserManager.FindByNameAsync(LModel.Email);

				if (user != null)
				{
					var identityResult = await signInManager.PasswordSignInAsync(user, LModel.Password,
						LModel.RememberMe, lockoutOnFailure: true);

					if (identityResult.Succeeded)
					{
						httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);

						var existingUserId = httpContextAccessor.HttpContext.Session.GetString("UserId");
						if (existingUserId != null && existingUserId != user.Id)
						{
							ModelState.AddModelError("LModel.Email", "Multiple logins detected. Please logout from other sessions.");
							await signInManager.SignOutAsync();
							return Page();
						}

						return RedirectToPage("Index");
					}
					if (identityResult.IsLockedOut)
					{
						ModelState.AddModelError("LModel.Email", "Account locked out. Please try again later.");
						return Page();
					}
				}
				ModelState.AddModelError("LModel.Email", "Username or Password incorrect");
			}
			return Page();
		}

		public bool ValidateCaptcha()
		{
			bool result = true;

			string captchaResponse = Request.Form["g-recaptcha-response"];

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LeUl2EpAAAAAMjWxA8uRShAl8ZkI1s9_dtBbBmi &response=" + captchaResponse);

			try
			{
				using (WebResponse wResponse = req.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();

						MyObject jsonObject = JsonSerializer.Deserialize<MyObject>(jsonResponse);
						result = Convert.ToBoolean(jsonObject.success);
					}
				}
				return result;
			}
			catch (WebException ex)
			{
				throw ex;
			}
		}
	}
}
