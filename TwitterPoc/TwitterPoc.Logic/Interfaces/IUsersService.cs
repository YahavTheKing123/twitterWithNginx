using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Logic.Interfaces
{
    public interface IUsersService
    {
        Task<Entities.User> GetVerifiedUserAsync(string username, string password);
        Task RegisterAsync(string username, string password);
        Task AddFollower(string follower, string followee);
        Task RemoveFollower(string follower, string followee);
    }
}
