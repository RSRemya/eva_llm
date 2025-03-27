using Eva_Repository.Auth;
using Eva_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : EvaPage
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager):base(signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterFormModel RegisterFormModel { get; set; }

        public string MainErrorMessage { get; set; }
        public string EmailErrorMessage { get; set; }
        public string PasswordErrorMessage { get; set; }
        public string ConfirmPasswordErrorMessage { get; set; }

        [BindProperty]
        public string ScrollPosition { get; set; }

        public void OnGet()
        {
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];   
            if (action == "logout")
                return await base.OnPostAsync();
            return await ValidateData();
        }

        private async Task<IActionResult> ValidateData()
        {
            bool failCheck = false;

            if (string.IsNullOrEmpty(RegisterFormModel.emailText))
            {
                EmailErrorMessage = "ⓘ Email is required.";
                failCheck = true;
            }
            else
            {
                string pattern = @"^(?!(?:(?:.*@.*){2,}))[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(RegisterFormModel.emailText, pattern))
                {
                    EmailErrorMessage = "ⓘ Email is not valid. Email addresses should follow the format user@domain.com.";
                    failCheck = true;
                }
            }

            if (string.IsNullOrEmpty(RegisterFormModel.passwordText))
            {
                PasswordErrorMessage = "ⓘ Password is required.";
                failCheck = true;
            }

            if (string.IsNullOrEmpty(RegisterFormModel.confirmPasswordText))
            {
                ConfirmPasswordErrorMessage = "ⓘ Confirm Password is required.";
                failCheck = true;
            }

            if (!string.IsNullOrEmpty(RegisterFormModel.passwordText) && !string.IsNullOrEmpty(RegisterFormModel.confirmPasswordText))
            {
                if (RegisterFormModel.passwordText != RegisterFormModel.confirmPasswordText)
                {
                    ConfirmPasswordErrorMessage = "ⓘ Confirm Password does not match.";
                    failCheck = true;
                }
            }

            if (!failCheck)
            {
                return await RegisterData();
            }

            return Page();
        }

        private async Task<IActionResult> RegisterData()
        {
            var user = new ApplicationUser { UserName = RegisterFormModel.emailText.Trim(), Email = RegisterFormModel.emailText.Trim() };
            var result = await _userManager.CreateAsync(user, RegisterFormModel.passwordText.Trim());
            await _userManager.AddToRoleAsync(user, "User");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");

            }
            else
            {
                MainErrorMessage = "ⓘ Something went wrong... Please register  again "+result.Errors.First().Description;
                return Page();
            }
        }
    }
}
