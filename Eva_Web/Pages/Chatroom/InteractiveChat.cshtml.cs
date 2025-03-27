using Eva_Repository.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages.Chatroom
{
    public class InteractiveChatModel : EvaPage
    {
        public InteractiveChatModel(SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {
        }

        public void OnGet()
        {
        }
    }
}
