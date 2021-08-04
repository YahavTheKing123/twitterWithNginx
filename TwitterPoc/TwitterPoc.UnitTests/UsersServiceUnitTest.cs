using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Authorization;
using TwitterPoc.Logic;
using TwitterPoc.Logic.Services;
using TwitterPoc.UnitTests.Mocks;

namespace TwitterPoc.UnitTests
{
    [TestClass]
    public class UsersServiceUnitTest
    {
        
        [TestMethod]
        public async Task RegistrationAndLogicTest()
        {
            var username = "adi1111";
            var password = "123456";
            var usersService = new UsersService(new UsersRepositoryMock(), new FollowersRepositoryMock(), new MessagesRepositoryMock(), new LoggerMock());
            await usersService.RegisterAsync(username, password);
            var user = await usersService.GetVerifiedUserAsync(username, password);
            Assert.IsNotNull(user);
            Assert.AreEqual(username, user.Username);
        }

        [TestMethod]
        public async Task LoginOnIncorrectPasswordTest()
        {
            var username = "adi1111";
            var password = "123456";
            var usersService = new UsersService(new UsersRepositoryMock(), new FollowersRepositoryMock(), new MessagesRepositoryMock(), new LoggerMock());
            await usersService.RegisterAsync(username, password);
            var user = await usersService.GetVerifiedUserAsync(username, "11111");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginOnIncorrectUsernameTest()
        {
            var username = "adi1111";
            var password = "123456";
            var usersService = new UsersService(new UsersRepositoryMock(), new FollowersRepositoryMock(), new MessagesRepositoryMock(), new LoggerMock());
            await usersService.RegisterAsync(username, password);
            var user = await usersService.GetVerifiedUserAsync("wrongUser", password);
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task AddFollowerTest()
        {
            const string user1 = "user1";
            const string user2 = "user2";
            const string user3 = "user3";
            const string user4 = "user4";
            var followersRepositoryMock =  new FollowersRepositoryMock();
            var usersRepositoryMock = new UsersRepositoryMock();
            var usersService = new UsersService(usersRepositoryMock, followersRepositoryMock, new MessagesRepositoryMock(), new LoggerMock());
            await Task.WhenAll(
                usersService.RegisterAsync(user1, "1111"),
                usersService.RegisterAsync(user2, "1111"),
                usersService.RegisterAsync(user3, "1111"),
                usersService.RegisterAsync(user4, "1111"));

            await Task.WhenAll(
                usersService.AddFollower(user1, user2),
                usersService.AddFollower(user1, user3),
                usersService.AddFollower(user2, user4)
                );

            var followees = (await followersRepositoryMock.Get(user1)).ToList();
            Assert.IsTrue(followees.Contains(user2));
            Assert.IsTrue(followees.Contains(user3));
            Assert.IsFalse(followees.Contains(user4));
            Assert.IsFalse(followees.Contains(user1));

            followees = (await followersRepositoryMock.Get(user2)).ToList();

            Assert.IsTrue(followees.Contains(user4));
            Assert.IsFalse(followees.Contains(user3));
            Assert.IsFalse(followees.Contains(user2));
            Assert.IsFalse(followees.Contains(user1));
        }

        [TestMethod]
        public async Task RemoveFollowerTest()
        {
            const string user1 = "user1";
            const string user2 = "user2";
            const string user3 = "user3";
            const string user4 = "user4";
            var followersRepositoryMock = new FollowersRepositoryMock();
            var usersRepositoryMock = new UsersRepositoryMock();
            var usersService = new UsersService(usersRepositoryMock, followersRepositoryMock, new MessagesRepositoryMock(), new LoggerMock());
            await Task.WhenAll(
                usersService.RegisterAsync(user1, "1111"),
                usersService.RegisterAsync(user2, "1111"),
                usersService.RegisterAsync(user3, "1111"),
                usersService.RegisterAsync(user4, "1111"));

            await Task.WhenAll(
                usersService.AddFollower(user1, user2),
                usersService.AddFollower(user1, user3),
                usersService.AddFollower(user2, user4)
                );

            await Task.WhenAll(
                usersService.RemoveFollower(user1, user2),
                usersService.RemoveFollower(user2, user4));

            var followees = (await followersRepositoryMock.Get(user1)).ToList();
            Assert.IsFalse(followees.Contains(user2));
            Assert.IsTrue(followees.Contains(user3));

        }




    }
}
