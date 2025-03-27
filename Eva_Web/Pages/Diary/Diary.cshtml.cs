using Eva_Repository;
using Eva_Repository.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class DiaryModel : EvaPage
    {


        public IList<DiaryEntry> DiaryEntries { get; private set; }

        public DiaryModel(SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {

        }

        public async Task OnGetAsync()
        {
            using var context = new EvaDbContext();
            DiaryEntries = await context.DiaryEntries.ToListAsync();
        }
    }
}
