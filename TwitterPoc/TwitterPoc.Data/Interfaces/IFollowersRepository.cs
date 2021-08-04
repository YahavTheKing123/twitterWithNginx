using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;

namespace TwitterPoc.Data.Interfaces
{
    public interface IFollowersRepository
    {
        Task Add(string follower, bool ignoreKeyDuplication);
        Task Add(string follower, string followee);
        Task Remove(string follower, string followee);

        Task<IEnumerable<string>> Get(string followerUsername); 
    }
}
