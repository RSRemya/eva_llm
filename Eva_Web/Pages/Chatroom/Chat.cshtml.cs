using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class Chat : EvaPage
    {
        [BindProperty]
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public string UserMessage { get; set; }

        public string UserId { get; set; }

        public Chat(SignInManager<ApplicationUser> signInManager) : base(signInManager)
        { 
        }


        public void OnGet(string id, string name)
        {
            UserId = id ?? HttpContext.Session.GetString("UserId");
            Messages = ConversationRepository.LoadUserConversations(Guid.Parse(UserId)).Where(conv => !conv.Conversation.Contains("system")).Select(n => { try { return JsonConvert.DeserializeObject<MessageModel>(n.Conversation); } catch { return new MessageModel { Sender = ExtractRoleField(n.Conversation), Text = ExtractContentField(n.Conversation) }; } }).Where(n => n != null).ToList();

        }

        private   string ExtractContentField(string json)
        { 
            return json.Substring(json.IndexOf("content\":") + 9, json.Length - json.IndexOf("content\":") - 10).Remove(0, 2).TrimEnd('\"'); 
        }
        private   string ExtractRoleField(string json)
        {
            // Regex pattern to match the role value inside the "role" field
            string pattern = "(?<=\\\"role\\\":\\s*\")(.*?)(?=\")";

            // Use Regex to extract the role field (raw string inside the quotes)
            Match match = Regex.Match(json, pattern);

            // Return the raw role (if found)
            return match.Success ? match.Value : string.Empty;


        }
    }
}