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
            var timeOfTest = DateTime.UtcNow;

            const string someMessage = "Some Message";
            const string messageOfUser3 = "Some Message of User 3";
            var usersRepositoryMock = new UsersRepositoryMock();
            var followersRepositoryMock = new FollowersRepositoryMock();
            var messagesRepositoryMock = new MessagesRepositoryMock();
            var feedsService = new FeedsService(messagesRepositoryMock, followersRepositoryMock);

            await Task.WhenAll(
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user1 }),
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user2 }),
                followersRepositoryMock.Add(user2, user1),
                feedsService.AddMessage(user1, someMessage),
                feedsService.AddMessage(user3, messageOfUser3)
                );

            var feed = await feedsService.GetUserFeed(user2, user1.Remove(user1.Length-2));
            var messages = feed.Messages.ToList();
            Assert.AreEqual(1, messages.Count);
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

            var timeOfTest = DateTime.UtcNow;
            var usersRepositoryMock = new UsersRepositoryMock();
            var followersRepositoryMock = new FollowersRepositoryMock();
            var messagesRepositoryMock = new MessagesRepositoryMock();
            var feedsService = new FeedsService(messagesRepositoryMock, followersRepositoryMock);

            await Task.WhenAll(
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user1 }),
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user2 }),
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user3 }),
                usersRepositoryMock.AddAsync(new Data.Entities.User() { Username = user4 }),
                feedsService.AddMessage(user1, someMessage + user1),
                feedsService.AddMessage(user2, someMessage + user2),
                feedsService.AddMessage(user3, someMessage + user3),
                feedsService.AddMessage(user4, someMessage + user4)
                );

            var feed = await feedsService.GetGlobalFeed(user1.Remove(user1.Length - 2));
            var messages = feed.Messages;
            Assert.AreEqual(4, messages.Count);

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
