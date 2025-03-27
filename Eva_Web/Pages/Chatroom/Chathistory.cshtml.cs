using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class ChatHistory :  EvaPage
    { 

        public List<KeyValuePair<string, List<MessageModel>>> UserConversationHistory { get; set; } = new List<KeyValuePair<string, List<MessageModel>>>();
        //Day : Users: Conversations
        public List<KeyValuePair<DateTime,List<KeyValuePair<string, List<MessageModel>>>>> UserConversationHistoryByDay { get; set; } = new List<KeyValuePair<DateTime, List<KeyValuePair<string, List<MessageModel>>>>>();




        private readonly UserManager<ApplicationUser> _userManager;
      
        public ChatHistory(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {
            this._userManager = _userManager;
        }

        public IActionResult OnGet(string date)
        {

            if (Request.Query.ContainsKey("delete"))
            {
                var user = _userManager.Users.FirstOrDefault(user => user.UserName == Request.Query["delete"].ToString().Trim());
                if (user != null)
                    ConversationRepository.DeleteAllUserConversations(Guid.Parse(user.Id));
               return RedirectToPage("/Index");

            }
            else
            {
                var convHistory = string.IsNullOrEmpty(date) ? ConversationRepository.LoadAllUserConversationsHistory() : ConversationRepository.LoadAllUserConversationsHistory().Where(conv => conv.Date.Date == DateTime.Parse(date).Date);

                foreach (var convsbydate in convHistory.GroupBy(conv => conv.Date.Date))
                {
                    var sortedconvsbydate = convsbydate.OrderByDescending(convsbydate => convsbydate.Date);
                    //for below date
                    var currentDate = convsbydate.Key;
                    //group user conversations on above date
                    var usersWithConversationsOnDate = sortedconvsbydate.GroupBy(conv => conv.UserId);

                    //to hold all users and their conversations, to be held by a parent container that has date, this should be sorted based on date
                    List<KeyValuePair<string, List<MessageModel>>> usersAndTheirConversationOnADate = new List<KeyValuePair<string, List<MessageModel>>>();

                    foreach (var userconvOnDate in usersWithConversationsOnDate.OrderByDescending(conv => conv.Max(e => e.Date)))
                    {
                        var sorteduserconvOnDate = userconvOnDate.OrderBy(convsbydate => convsbydate.Date);//sorts conversation at user level
                        var user = _userManager.Users.FirstOrDefault(user => user.Id == userconvOnDate.Key.ToString())?.UserName;
                        if (user != null)
                        {
                            var userconversation = sorteduserconvOnDate
                                .Select(n =>
                                {
                                    try
                                    {
                                        return JsonConvert.DeserializeObject<MessageModel>(n.Conversation);
                                    }
                                    catch
                                    {
                                        return new MessageModel { Sender = ExtractRoleField(n.Conversation), Text = ExtractContentField(n.Conversation) };
                                    }
                                })
                                .Where(n => n != null && n.Sender != "system")
                                .ToList();
                            usersAndTheirConversationOnADate.Add(new KeyValuePair<string, List<MessageModel>>(user, userconversation));
                        }

                    }


                    UserConversationHistoryByDay.Add(new KeyValuePair<DateTime, List<KeyValuePair<string, List<MessageModel>>>>(currentDate, usersAndTheirConversationOnADate));

                }

                UserConversationHistoryByDay = UserConversationHistoryByDay.OrderByDescending(u => u.Key).ToList();
               
            }
            return Page();
        }

        private string ExtractContentField(string json)
        {
            return json.Substring(json.IndexOf("content\":") + 9, json.Length - json.IndexOf("content\":") - 10).Remove(0, 2).TrimEnd('\"');
        }
        private string ExtractRoleField(string json)
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