using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;

namespace TwitterPoc.Data.Interfaces
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(string username);

        Task<bool> UserExists(string username);
    }
}
