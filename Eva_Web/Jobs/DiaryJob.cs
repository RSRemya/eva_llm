using Eva_Repository;
using LLMIntegration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Eva_Web.Jobs;

public class DiaryJob : BackgroundService
{ 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {   
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                CreateDiarySummaryForAllUsersForTodayTask();
            }
            catch { }

            // Wait for 24 hours 
           await Task.Delay(TimeSpan.FromHours(1), stoppingToken); 
        }
    }
    private async void CreateDiarySummaryForAllUsersForTodayTask()
    {
        DateTime conversationStartDay = ConversationRepository.LoadAllUserConversationsHistory().OrderBy(o => o.Date).First().Date;
        var datesincelastdiaryentry= DiaryRepository.LastDiaryEntryDate();
        if (datesincelastdiaryentry != null) //not fresh diary
            conversationStartDay = datesincelastdiaryentry.Value;

        if (conversationStartDay!=null && conversationStartDay.Date == DateTime.UtcNow.Date) //only create for past
            return; 
        
        for (var day = conversationStartDay; day <= DateTime.UtcNow; day=day.AddDays(1))
        {
            var userConversationsForDiarySummary = ConversationRepository.AllConversationsForADay(day).GroupBy(item => item.Key);
            foreach (var user in userConversationsForDiarySummary)
            {
                var entry = await LLM.DiarySummary(user.Select(u => u.Value).ToList());
                if (!string.IsNullOrEmpty(entry))
                    DiaryRepository.SaveOrUpdateDiary(new DiaryEntry
                    {
                        UserId = user.Key,
                        DiaryEntryDate = DateTime.UtcNow,
                        Entry = entry
                    });

            }
        }
    }
}
