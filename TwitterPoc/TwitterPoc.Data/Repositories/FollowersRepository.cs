using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Interfaces;

namespace TwitterPoc.Data.Repositories
{
    public class FollowersRepository : IFollowersRepository
    {
        private readonly Dictionary<string, HashSet<string>> _userFollowees = new Dictionary<string, HashSet<string>>();

        public async Task Add(string follower, string followee)
        {
            await Task.Delay(1);
            if (!_userFollowees.ContainsKey(follower))
            {
                _userFollowees.Add(follower, new HashSet<string>());
            }

            _userFollowees[follower].Add(followee);
        }

        public async Task Remove(string follower, string followee)
        {
            await Task.Delay(1);
            if (!_userFollowees.ContainsKey(follower))
            {
                _userFollowees.Add(follower, new HashSet<string>());
            }

            _userFollowees[follower].Remove(followee);
        }

        public async Task<IEnumerable<string>> Get(string followerUsername)
        {
            await Task.Delay(1);
            if (_userFollowees.ContainsKey(followerUsername))
            {
                return _userFollowees[followerUsername].ToList();
            }
            return Enumerable.Empty<string>();
        }
    }
}
