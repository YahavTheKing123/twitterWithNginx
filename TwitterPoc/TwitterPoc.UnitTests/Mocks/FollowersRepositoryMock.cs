using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Interfaces;

namespace TwitterPoc.UnitTests.Mocks
{
    public class FollowersRepositoryMock : IFollowersRepository
    {
        private readonly Dictionary<string, HashSet<string>> _userFollowees = new Dictionary<string, HashSet<string>>();

        public async Task Add(string follower, bool ignoreKeyDuplication)
        {
            await Task.FromResult(0);
            if (ignoreKeyDuplication && _userFollowees.ContainsKey(follower))
            {
                return;
            }
            _userFollowees.Add(follower, new HashSet<string>());
        }

        public async Task Add(string follower, string followee)
        {
            await Task.FromResult(0);

            _userFollowees[follower].Add(followee);
        }

        public async Task Remove(string follower, string followee)
        {
            await Task.FromResult(0);

            _userFollowees[follower].Remove(followee);
        }

        public async Task<IEnumerable<string>> Get(string followerUsername)
        {
            await Task.FromResult(0);
            if (_userFollowees.ContainsKey(followerUsername))
            {
                return _userFollowees[followerUsername].ToList();
            }
            return Enumerable.Empty<string>();
        }

    }
}
