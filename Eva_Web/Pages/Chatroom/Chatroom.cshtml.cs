using Eva_Repository;
using Eva_Repository.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin,User")]
    [Obsolete]//botstack
    public class ChatroomModel : EvaPage
    {
        public string apiKey { get; set; }
        public string botId { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }

        public ChatroomModel( SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {
        
        }
        public void OnGet(string id, string name)
        {
            UserId = id;
            Name = name;
            // the below code is needed in a scenario where we want to decouple the bot config from UI,
            // in our case the below is not strictly necessary as the apikey and botid will be same for 
            // all users in our case, also BotStackConfigModels has a FK reference to the  Users table
            // which is not needed anymore, another fact is that there is no UI screen for configuring 
            // botstack config, so any configuration will require manipulating the SQL server records
            // hence commenting out the below
            /*using var context = new EvaDbContext();
            var config = context.BotStackConfigModels.FirstOrDefault(config => config.UserId.Value.ToString() == UserId);
            if (config != null)
            {
                apiKey = config.ApiKey;
                botId = config.BotId.ToString();
            }*/
        }
    }
}
