using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eva_Repository
{
    public static class DiaryRepository
    {
        public static bool SaveOrUpdateDiary(DiaryEntry diaryEntry)
        {
            if (diaryEntry == null) return false;
            using (var context = new EvaDbContext())
            {
                Guid userIdFromBotstack = diaryEntry.UserId;
                string userDiaryEntry = diaryEntry.Entry;

                var userFromDB = context.Users.Any(user => user.UserId == userIdFromBotstack) ? context.Users.First(user => user.UserId == userIdFromBotstack) : null;
                if (userFromDB != null)
                { //update diary entry 

                    var diaryEntryForUser = context.DiaryEntries.Any(entry => entry.UserId == userFromDB.UserId) ? context.DiaryEntries.First(entry => entry.UserId == userFromDB.UserId) : null;
                    if (diaryEntryForUser != null&&diaryEntryForUser.DiaryEntryDate.Date==diaryEntry.DiaryEntryDate.Date)
                    {
                        diaryEntryForUser.Entry = userDiaryEntry;
                        context.SaveChanges();
                    }
                    else
                    {
                        var diaryEntryNewForExistingUser = new DiaryEntry { UserId = userFromDB.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = userDiaryEntry };
  
                        context.DiaryEntries.Add(diaryEntryNewForExistingUser);
                        context.SaveChanges();

                    }
                    return true;
                }
                //save new user and make first entry
                var user = new User { UserId = userIdFromBotstack, UserAnonymousName = "AnonymousUser1" };
                var diaryEntryNew = new DiaryEntry { UserId = user.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = userDiaryEntry };


                context.Users.Add(user);
                context.DiaryEntries.Add(diaryEntryNew);
                context.SaveChanges();
                return true;
            }
        }

        public static string LastConversationContextForUser(Guid userId)
        {
            using var context = new EvaDbContext();
            return context.DiaryEntries.Any(entry => entry.UserId == userId)?context.DiaryEntries.Where(entry=>entry.UserId==userId).OrderByDescending(entry=>entry.DiaryEntryDate).First().Entry:string.Empty;             
        }
        public static DateTime? LastDiaryEntryDate()
        { 
            using (var context = new EvaDbContext()) {
             return context.DiaryEntries.OrderByDescending(diary=>diary.DiaryEntryDate).FirstOrDefault()?.DiaryEntryDate.Date; 
            }
           
        }

        public static string AllDiaryEntriesForToday()
        {
            string combinedDiaryEntryForToday = "";
            var currentDate = DateTime.Now;
            using (var context = new EvaDbContext())
            {
                if (context.DiaryEntries.Any(entry => entry.DiaryEntryDate.Date == currentDate.Date))
                    context.DiaryEntries.Where(entry => entry.DiaryEntryDate.Date == currentDate.Date).ForEachAsync(entry => { combinedDiaryEntryForToday += " " + entry.Entry; }).Wait();
            }
            return combinedDiaryEntryForToday;
        }

        public static List<DiaryEntry> ListAllDiaryEntries()
        {

            using var context = new EvaDbContext();
            return context.DiaryEntries.ToList();
           
        }

    }
}
