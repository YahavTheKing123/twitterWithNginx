using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;

namespace TwitterPoc.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly List<User> users = new List<User>();

        public UsersRepository()
        {
            users.Add(new User
            {
                Username = "adi",
                Password = "123",
            });
            users.Add(new User
            {
                Username = "michaelsanders",
                Password = "michael321",
            });
        }
        public async Task<User> GetUserAsync(string username, string password)
        {
            return await Task.Run(() =>users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault());
        }
    }
}
