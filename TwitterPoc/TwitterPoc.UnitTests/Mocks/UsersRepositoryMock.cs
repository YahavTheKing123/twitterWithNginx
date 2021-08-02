using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;

namespace TwitterPoc.UnitTests.Mocks
{
    public class UsersRepositoryMock : IUsersRepository
    {
        private readonly List<User> _users = new List<User>();

        public UsersRepositoryMock()
        {
            _users.Add(new User
            {
                Username = "adi",
                Password = "123",
            });
            _users.Add(new User
            {
                Username = "michaelsanders",
                Password = "michael321",
            });
        }
        public async Task<User> GetAsync(string username)
        {
            return await Task.Run(() =>_users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault());
        }

        public async Task AddAsync(User user)
        {
            await Task.Run(() => _users.Add(user));
        }
    }
}
