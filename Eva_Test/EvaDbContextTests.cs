using System;
using System.Linq;
using Eva_Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class EvaDbContextTests
{
    private DbContextOptions<EvaDbContext> GetSqlliteDbContextOptions()
    {
        return new DbContextOptionsBuilder<EvaDbContext>()
            .UseSqlServer(Guid.NewGuid().ToString())//dont use
            .Options;
    }

    [Fact]
    public void CanAddUser()
    {
        // Arrange
        var options = GetSqlliteDbContextOptions();
        using var context = new EvaDbContext(options);

        var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };

        // Act
        context.Users.Add(user);
        context.SaveChanges();

        // Assert
        Assert.Empty(context.Users);
        Assert.Equal("AnonymousUser1", context.Users.Single().UserAnonymousName);
    }

    [Fact]
    public void CanAddBotStackConfigModel()
    {
        // Arrange
        var options = GetSqlliteDbContextOptions();
        using var context = new EvaDbContext(options);

        var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };
        var botConfig = new BotStackConfigModel { BotId = Guid.NewGuid(), ApiKey = "SomeApiKey", UserId = user.UserId };

        // Act
        context.Users.Add(user);
        context.BotStackConfigModels.Add(botConfig);
        context.SaveChanges();

        // Assert
        Assert.Single(context.Users);
        Assert.Single(context.BotStackConfigModels);
        Assert.Equal("SomeApiKey", context.BotStackConfigModels.Single().ApiKey);
        Assert.Equal(user.UserId, context.BotStackConfigModels.Single().UserId);
    }

    [Fact]
    public void CanAddDiaryEntry()
    {
        // Arrange
        var options = GetSqlliteDbContextOptions();
        using var context = new EvaDbContext(options);

        var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };
        var diaryEntry = new DiaryEntry { UserId = user.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = "Today was a good day." };

        // Act
        context.Users.Add(user);
        context.DiaryEntries.Add(diaryEntry);
        context.SaveChanges();

        // Assert
        Assert.Equal(1, context.Users.Count());
        Assert.Equal(1, context.DiaryEntries.Count());
        Assert.Equal("Today was a good day.", context.DiaryEntries.Single().Entry);
        Assert.Equal(user.UserId, context.DiaryEntries.Single().UserId);
    }

    [Fact]
    public void CanAddDiaryEntryUserMightNotExistSaveFirstIfDoesntExist()
    {
        // Arrange
        var options = GetSqlliteDbContextOptions();
        using var context = new EvaDbContext(options);
        //first check if user exists or not
        Guid userIdFromBotstack = Guid.Parse("5aa1de7a-2b43-4a32-8b65-c35b46e366fe");//Guid.NewGuid();
        string userDiaryEntry = "Today i am fine 2";

       var userFromDB= context.Users.Any(user => user.UserId == userIdFromBotstack) ? context.Users.First(user => user.UserId == userIdFromBotstack):null;
        if (userFromDB != null)
        { //update diary entry

            //  context.DiaryEntries.Add(new DiaryEntry { UserId = userFromDB.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = userDiaryEntry });

            var diaryEntryForUser=context.DiaryEntries.Any(entry=>entry.UserId== userFromDB.UserId)? context.DiaryEntries.First(entry => entry.UserId == userFromDB.UserId):null;
            if (diaryEntryForUser != null)
            {
                diaryEntryForUser.Entry=userDiaryEntry;
               // diaryEntryForUser.DiaryEntryDate=DateTime.Now;
                context.SaveChanges();
            }
            return;
        }
        //save new user and make first entry
        var user = new User { UserId = userIdFromBotstack, UserAnonymousName = "AnonymousUser1" };
        var diaryEntry = new DiaryEntry { UserId = user.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = userDiaryEntry };

        // Act
        context.Users.Add(user);
        context.DiaryEntries.Add(diaryEntry);
        context.SaveChanges();

        // Assert
        Assert.Equal(1, context.Users.Count());
        Assert.Equal(1, context.DiaryEntries.Count());
        Assert.Equal("Today was a good day.", context.DiaryEntries.Single().Entry);
        Assert.Equal(user.UserId, context.DiaryEntries.Single().UserId);
    }
}
