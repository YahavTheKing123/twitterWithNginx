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
        

    }
}
