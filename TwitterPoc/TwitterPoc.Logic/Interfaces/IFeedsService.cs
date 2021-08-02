using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Logic.Entities;

namespace TwitterPoc.Logic.Interfaces
{
    public interface IFeedsService
    {
        Task AddMessage(string username, string message);
        Task<Feed> GetUserFeed(string currentUsername, string followeeUsername);
        Task<Feed> GetGlobalFeed(string followeePartialUsername);

    }
}
