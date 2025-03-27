using Eva_Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eva_Web.Models;
using Eva_Repository.Auth;
using Microsoft.AspNetCore.Identity;


namespace Eva_Web.Pages
{
    public class UsergeneratorModel : PageModel
    {
        public Dictionary<string, string> UserCredentials { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        public UsergeneratorModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {
            UserCredentials = CredentialGenerator.GenerateMultipleCredentials(5);
        }
        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            return await CreateUser(username, password);
        }

        private async Task<IActionResult> CreateUser(string username, string password)
        {
            var user = new ApplicationUser { UserName = username, Email = username+"@nomail.com" };
          
            try
            {
                var identity= await _userManager.CreateAsync(user, password);

                if (identity.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                } 
               
            }
            catch (Exception ex) { 
            }
            return RedirectToPage("/Authentication/Login"); 
        }
    }
}
