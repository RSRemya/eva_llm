using System.Text.RegularExpressions;
using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Eva_Web.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Eva_Web.Pages
{

    public class QrSignupModel : EvaPage
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public QrSignupModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {
            _userManager = userManager;

        }


        [BindProperty]
        public string UserKey { get; set; } 
         

        [BindProperty]
        public HomeEmailFormModel HomeEmailModel { get; set; }

        public string MainErrorMessage { get; set; }
        public string EmailErrorMessage { get; set; }
        public string MessageErrorMessage { get; set; }
        public string Message2ErrorMessage { get; set; }

        [BindProperty]
        public string ScrollPosition { get; set; }

        [BindProperty]
        public string DbStatus { get; set; }
        public void OnGet()
        {

        }

        public override async Task<IActionResult>  OnPostAsync()
        {
            var action = Request.Form["action"];  // captures the action of the user for the selected row
            if (action == "logout")
                return await base.OnPostAsync();
            return await ValidateData();
        }

        private async Task<IActionResult> ValidateData()
        {
            bool failCheck = false;

            if (string.IsNullOrEmpty(HomeEmailModel.emailText))
            {
                EmailErrorMessage = "ⓘ Email is required.";
                failCheck = true;
            }
            else
            {
                string pattern = @"^(?!(?:(?:.*@.*){2,}))[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(HomeEmailModel.emailText, pattern))
                {
                    EmailErrorMessage = "ⓘ Email is not valid. Email addresses should follow the format user@domain.com.";
                    failCheck = true;
                }
            }

            if (string.IsNullOrEmpty(HomeEmailModel.messageText))
            {
                MessageErrorMessage = "ⓘ Why do you want to talk to Eva? is required.";
                failCheck = true;
            }

            if (string.IsNullOrEmpty(HomeEmailModel.messageText2))
            {
                Message2ErrorMessage = "ⓘ Why should you be given access to Eva? is required.";
                failCheck = true;
            }

            if (!failCheck)
            {
                try
                {
                    Eva_Repository.UserRequest usertRequest = new Eva_Repository.UserRequest();
                    usertRequest.UserEmail = HomeEmailModel.emailText;
                    usertRequest.ReasonforEva = HomeEmailModel.messageText;
                    usertRequest.AccesstoEva = HomeEmailModel.messageText2;
                    var stats = UserRepository.SaveOrUpdateUser(usertRequest);

                    if (stats)
                    {
                        DbStatus = "success";
                        return await email();
                    }
                    else
                    {
                        DbStatus = "failed";
                        dataClearing();
                    }
                }
                catch
                {
                    DbStatus = "failed";
                    dataClearing();
                }
            }
            return Page();
        }

        private async Task<IActionResult> email()
        {
            var sendEmail = new SendEmail();
            sendEmail.send("suyogamin@gmail.com", $"Invitation from {HomeEmailModel.emailText.Trim().Split("@")[0]}", $"Email - {HomeEmailModel.emailText.Trim()} \n\n Message - {HomeEmailModel.messageText.Trim()} \n\n {HomeEmailModel.messageText2.Trim()}");
            dataClearing();

            return Page();
        }

        private void dataClearing()
        {
            ModelState.Clear();
            HomeEmailModel.emailText = String.Empty;
            HomeEmailModel.messageText = String.Empty;
            HomeEmailModel.messageText2 = String.Empty;
        }
    }

}
