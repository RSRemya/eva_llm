using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YourNamespace.Tests
{
    [TestClass]
    public class MyDbContextTests
    {
        private DbContextOptions<MyDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void CanAddUser()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new MyDbContext(options);

            var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };

            // Act
            context.Users.Add(user);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Users.Count());
            Assert.AreEqual("AnonymousUser1", context.Users.Single().UserAnonymousName);
        }

        [TestMethod]
        public void CanAddBotStackConfigModel()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new MyDbContext(options);

            var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };
            var botConfig = new BotStackConfigModel { BotId = Guid.NewGuid(), ApiKey = "SomeApiKey", UserId = user.UserId };

            // Act
            context.Users.Add(user);
            context.BotStackConfigModels.Add(botConfig);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Users.Count());
            Assert.AreEqual(1, context.BotStackConfigModels.Count());
            Assert.AreEqual("SomeApiKey", context.BotStackConfigModels.Single().ApiKey);
            Assert.AreEqual(user.UserId, context.BotStackConfigModels.Single().UserId);
        }

        [TestMethod]
        public void CanAddDiaryEntry()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new MyDbContext(options);

            var user = new User { UserId = Guid.NewGuid(), UserAnonymousName = "AnonymousUser1" };
            var diaryEntry = new DiaryEntry { UserId = user.UserId, DiaryEntryDate = DateTime.UtcNow, Entry = "Today was a good day." };

            // Act
            context.Users.Add(user);
            context.DiaryEntries.Add(diaryEntry);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Users.Count());
            Assert.AreEqual(1, context.DiaryEntries.Count());
            Assert.AreEqual("Today was a good day.", context.DiaryEntries.Single().Entry);
            Assert.AreEqual(user.UserId, context.DiaryEntries.Single().UserId);
        }
    }
}
