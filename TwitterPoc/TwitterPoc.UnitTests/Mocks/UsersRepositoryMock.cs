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
        }
        public async Task<User> GetAsync(string username)
        {
            return await Task.Run(() =>_users.Where(x => x.Username == username).FirstOrDefault());
        }

        public async Task AddAsync(User user)
        {
            await Task.Run(() => _users.Add(user));
        }

        public async Task<bool> UserExists(string username)
        {
            await Task.FromResult(0);
            return _users.Any(u => u.Username == username);
        }
    }
}
