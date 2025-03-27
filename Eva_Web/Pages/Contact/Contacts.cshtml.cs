using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Eva_Web.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Eva_Web.Pages
{
    public class ContactsModel : EvaPage
    {
       

        [BindProperty]
        public EmailFormModel EmailModel { get; set; }

        public string MainErrorMessage { get; set; }
        public string NameErrorMessage { get; set; }
        public string FirstNameErrorMessage { get; set; }
        public string LastNameErrorMessage { get; set; }
        public string EmailErrorMessage { get; set; }
        public string MessageErrorMessage { get; set; }

        [BindProperty]
        public string ScrollPosition { get; set; }

        [BindProperty]
        public string DbStatus { get; set; }

        public void OnGet()
        {

        }
        public ContactsModel(SignInManager<ApplicationUser> signInManager) : base(signInManager)
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

            if (string.IsNullOrEmpty(EmailModel.firstNameText) && string.IsNullOrEmpty(EmailModel.lastNameText))
            {
                NameErrorMessage = "ⓘ Name is required.";
                failCheck = true;
            }
            else
            {
                if (string.IsNullOrEmpty(EmailModel.firstNameText))
                {
                    FirstNameErrorMessage = "ⓘ First Name is required.";
                    failCheck = true;
                }
                if (string.IsNullOrEmpty(EmailModel.lastNameText))
                {
                    LastNameErrorMessage = "ⓘ Last Name is required.";
                    failCheck = true;
                }
            }

            if (string.IsNullOrEmpty(EmailModel.emailText))
            {
                EmailErrorMessage = "ⓘ Email is required.";
                failCheck = true;

            }
            else
            {
                string pattern = @"^(?!(?:(?:.*@.*){2,}))[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(EmailModel.emailText, pattern))
                {
                    EmailErrorMessage = "ⓘ Email is not valid. Email addresses should follow the format user@domain.com.";
                    failCheck = true;
                }
            }


            if (string.IsNullOrEmpty(EmailModel.messageText))
            {
                MessageErrorMessage = "ⓘ Message is required.";
                failCheck = true;
            }

            if (!failCheck)
            {
                try
                {
                    Eva_Repository.Contacts contactRequest = new Eva_Repository.Contacts();
                    contactRequest.FirstName = EmailModel.firstNameText;
                    contactRequest.LastName = EmailModel.lastNameText;
                    contactRequest.Email = EmailModel.emailText;
                    contactRequest.Message = EmailModel.messageText;
                    contactRequest.ShowHide = false;
                    var stats = UserRepository.SaveOrUpdateContact(contactRequest);

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
            sendEmail.send("suyogamin@gmail.com", $"Invitation from {EmailModel.firstNameText.Trim()} {EmailModel.lastNameText.Trim()}", $"Email - {EmailModel.emailText.Trim()} \n\n Message - {EmailModel.messageText.Trim()}");
            dataClearing();
            
            return Page();
        }

        private void dataClearing()
        {
            ModelState.Clear();
            EmailModel.firstNameText = String.Empty;
            EmailModel.lastNameText = String.Empty;
            EmailModel.emailText = String.Empty;
            EmailModel.messageText = String.Empty;
        }
    }
}

