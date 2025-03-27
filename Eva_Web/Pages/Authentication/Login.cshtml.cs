using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Eva_Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public bool IsAdmin
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Session.GetString("IsUserAdmin") ?? "false");
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            }
        }
        
        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginFormModel LoginFormModel { get; set; }

        public string MainErrorMessage { get; set; }
        public string EmailErrorMessage { get; set; }
        public string PasswordErrorMessage { get; set; }

        [BindProperty]
        public string ScrollPosition { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
           return await ValidateData();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.Keys.Any()) // already a session is active, redirect to index page
            {
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }
        }

        private async Task<IActionResult> ValidateData()
        {
            bool failCheck = false;

            if (string.IsNullOrEmpty(LoginFormModel.userNameText))
            {
                EmailErrorMessage = "ⓘ User Name is required.";
                failCheck = true;
            }

            if (string.IsNullOrEmpty(LoginFormModel.passwordText))
            {
                PasswordErrorMessage = "ⓘ Password is required.";
                failCheck = true;
            }

            if (!failCheck)
            {
                return await VerifyData();
            }

            return Page();
        }
        private async Task<IActionResult> VerifyData()
        {
            var result = await _signInManager.PasswordSignInAsync(LoginFormModel.userNameText.Trim(), LoginFormModel.passwordText.Trim(), isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                HttpContext.Session.Clear();
                var user = _signInManager.UserManager.Users.First(user => user.UserName == LoginFormModel.userNameText.Trim());

                HttpContext.Session.SetString("UserId", user.Id);
                bool IsAdmin = _signInManager.UserManager.IsInRoleAsync(user, "Admin").Result;
                HttpContext.Session.SetString("IsUserAdmin", IsAdmin.ToString());
                return IsAdmin ? RedirectToPage("/Index", new { id = user.Id, name = LoginFormModel.userNameText.Trim() }) : RedirectToPage("/Chatroom/Chat", new { id = user.Id, name = LoginFormModel.userNameText.Trim() });
            }
            else
            {
                MainErrorMessage = "ⓘ Wrong Email or Password... Please try again";
                return Page();
            }
        }


    }

}
