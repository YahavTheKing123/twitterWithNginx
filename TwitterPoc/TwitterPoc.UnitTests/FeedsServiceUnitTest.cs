using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Authorization;
using TwitterPoc.Logic;
using TwitterPoc.Logic.Services;
using TwitterPoc.UnitTests.Mocks;

namespace TwitterPoc.UnitTests
{
    [TestClass]
    public class FeedsServiceUnitTest
    {
        
        [TestMethod]
        [Timeout(5000)] //5 seconds timeout for message time measurement
        public async Task GetMyFeedTest()
        {
            const string user1 = "user1";
            const string user2 = "user2";
            const string user3 = "other3";
            const string somePassword = "Aa132442";
            var timeOfTest = DateTime.UtcNow;

            const string someMessage = "Some Message";
            const string messageOfUser3 = "Some Message of User 3";
            var usersRepositoryMock = new UsersRepositoryMock();
            var followersRepositoryMock = new FollowersRepositoryMock();
            var messagesRepositoryMock = new MessagesRepositoryMock();
            var usersService = new UsersService(usersRepositoryMock, followersRepositoryMock, messagesRepositoryMock, new LoggerMock());
            var feedsService = new FeedsService(messagesRepositoryMock, followersRepositoryMock, new ConfigurationMock(), new LoggerMock());

            await Task.WhenAll(
                usersService.RegisterAsync(user1, somePassword),
                usersService.RegisterAsync(user2, somePassword)
            );
            await Task.WhenAll(
                followersRepositoryMock.Add(user2, user1),
                feedsService.AddMessage(user1, someMessage),
                feedsService.AddMessage(user3, messageOfUser3)
                );

            var feed = await feedsService.GetUserFeed(user2, user1.Remove(user1.Length-2));
            var messages = feed.Messages.ToList();
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, feed.Followees.Count);
            Assert.AreEqual(user1, feed.Followees.First());
            var singleMessage = messages.First();

            Assert.AreEqual(someMessage, singleMessage.Content);
            Assert.AreEqual(user1, singleMessage.Username);

            Assert.IsTrue(singleMessage.Time <= timeOfTest.AddSeconds(1) && singleMessage.Time > timeOfTest.AddSeconds(-1));

        }

        [TestMethod]
        [Timeout(5000)] //5 seconds timeout for message time measurement
        public async Task GetGlobalFeedTest()
        {
            const string user1 = "user1";
            const string user2 = "user2";
            const string user3 = "user3";
            const string user4 = "user4";
            const string someMessage = "Some Message @";
            const string somePassword = "Aa132442";

            var timeOfTest = DateTime.UtcNow;
            var usersRepositoryMock = new UsersRepositoryMock();
            var followersRepositoryMock = new FollowersRepositoryMock();
            var messagesRepositoryMock = new MessagesRepositoryMock();
            var usersService = new UsersService(usersRepositoryMock, followersRepositoryMock, messagesRepositoryMock, new LoggerMock());
            var feedsService = new FeedsService(messagesRepositoryMock, followersRepositoryMock, new ConfigurationMock(), new LoggerMock());

            await Task.WhenAll(
                usersService.RegisterAsync(user1, somePassword),
                usersService.RegisterAsync(user2, somePassword),
                usersService.RegisterAsync(user3, somePassword),
                usersService.RegisterAsync(user4, somePassword)
                );

            await Task.WhenAll(
                feedsService.AddMessage(user1, someMessage + user1),
                feedsService.AddMessage(user2, someMessage + user2),
                feedsService.AddMessage(user3, someMessage + user3),
                feedsService.AddMessage(user4, someMessage + user4)
                );

            var feed = await feedsService.GetGlobalFeed(user2, user1.Remove(user1.Length - 2));
            var messages = feed.Messages;
            Assert.AreEqual(4, messages.Count);
            Assert.AreEqual(0, feed.Followees.Count);

            foreach (var message in messages)
            {
                var expectedUsername = message.Content.Split("@")[1];
                Assert.AreEqual(expectedUsername, message.Username);
                Assert.AreEqual(someMessage + message.Username, message.Content);
                Assert.IsTrue(message.Time <= timeOfTest.AddSeconds(1) && message.Time > timeOfTest.AddSeconds(-1));
            }
        }


    }
}
