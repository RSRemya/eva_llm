using Eva_Repository;
using Eva_Repository.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class DemoroomModel : EvaPage
    {            
        public string apiKey { get; set; }
        public string botId { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }

        public DemoroomModel(SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {

        }
        public void OnGet()
        {
            UserId = Guid.NewGuid().ToString();
            Name = "Demo_User"; 
        }
    }
}
