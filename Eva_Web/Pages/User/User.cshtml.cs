using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages
{
    public class UserModel : PageModel
    {
        public string UserKey { get; set; }
        public void OnGet()//create userkey and also provision a new chatbot id, if available
        {
            UserKey = Path.GetRandomFileName();
        }
    }
}
