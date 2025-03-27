using Eva_Repository.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages
{
    public class EvaPage : PageModel
    {
        public bool IsAdmin
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Session.GetString("IsUserAdmin") ?? "false");
            }
        }

        protected SignInManager<ApplicationUser> signInManager;
        // pages that need signout needs to use this constructor
        public EvaPage(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public EvaPage()
        { }


        public bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            }
        }



        public string UserId
        {
            get
            {
                return  HttpContext.Session.GetString("UserId");
            }
        }
        public virtual async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];

            if (action == "logout")
            {
                bool isloggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
                if (isloggedIn)
                {
                    await this.signInManager.SignOutAsync();
                    HttpContext.Session.Remove("UserId");
                    HttpContext.Session.Remove("IsUserAdmin");
                }
              
            }
            return RedirectToPage("/Authentication/Login");
        }
    }
}
