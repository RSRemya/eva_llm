using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eva_Repository
{
    public static  class ConversationRepository
    {

        public static bool SaveUserConversation(Guid userId,string  lastConversationLine)
        {
            using (var context = new EvaDbContext())
            {
                context.Conversations.Add(new ConversationContext { UserId = userId, Conversation = lastConversationLine });
                context.SaveChanges();
                return true;
            }
        }

        public static bool SaveUserConversationContexts(Guid userId, ConversationContext[] conversationContexts)
        {
            using (var context = new EvaDbContext())
            {
                context.Conversations.AddRange(conversationContexts);
                context.SaveChanges();
                return true;
            }
        }
        public static List<ConversationContext> LoadUserConversations(Guid userId)
        {
            using (var context = new EvaDbContext())
            {
                return context.Conversations.Where(conv=>conv.UserId==userId)?.ToList(); 
            }
        }
        public static bool DeleteAllUserConversations(Guid userId)
        {
            using (var context = new EvaDbContext())
            {
                return context.Conversations.Where(conv => conv.UserId == userId).ExecuteDelete() > 0;
            }
        }
        public static List<ConversationContext> LoadAllUserConversationsHistory()
        {
            using (var context = new EvaDbContext())
            {
                return context.Conversations.ToList();
            }
        }

        public static List<KeyValuePair<Guid, string>> AllConversationsForADay(DateTime day)
        {

            var currentDate = day;
            using (var context = new EvaDbContext())
            {
                return context.Conversations?.Where(entry => entry.Date.Date == currentDate.Date)?.Select(conv => KeyValuePair.Create(conv.UserId, conv.Conversation))?.ToList();
            }

        }

        public static List<KeyValuePair<Guid,string>> AllConversationsForToday()
        {
           
            var currentDate = DateTime.UtcNow;
            using (var context = new EvaDbContext())
            {
              return context.Conversations?.Where(entry => entry.Date.Date == currentDate.Date)?.Select(conv =>  KeyValuePair.Create(conv.UserId,conv.Conversation))?.ToList();  
            }
           
        }
        public static List<string> AllUserConversationsForToday(Guid userId)
        { 
            var currentDate = DateTime.UtcNow;
            using (var context = new EvaDbContext())
            { 
                return context.Conversations?.Where(entry => entry.UserId==userId && entry.Date.Date == currentDate.Date)?.Select(conv => conv.Conversation)?.ToList();
            } 
        }
    }
}
