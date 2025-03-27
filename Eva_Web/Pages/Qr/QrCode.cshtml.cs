using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Eva_Web.Pages
{
    public class QrModel : EvaPage
    {
        public void OnGet()
        {
            
        }

         public async override Task<IActionResult> OnPostAsync()
        {
           return RedirectToPage("/Qr/QrSignup"); 
        }
    }

}
