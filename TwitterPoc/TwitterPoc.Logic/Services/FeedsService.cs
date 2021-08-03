using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Logic.Entities;
using TwitterPoc.Logic.Interfaces;

namespace TwitterPoc.Logic.Services
{
    public class FeedsService : IFeedsService
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly IFollowersRepository _followersRepository;

        public FeedsService(IMessagesRepository messagesRepository, IFollowersRepository followersRepository)
        {
            _messagesRepository = messagesRepository;
            _followersRepository = followersRepository;
        }

        public async Task AddMessage(string username, string message)
        {
            var messageToAdd = new Data.Entities.Message() {
                Content = message,
                Time = DateTime.UtcNow,
            };
            await _messagesRepository.Add(username, messageToAdd);
        }

        public async Task<Feed> GetGlobalFeed(string followeePartialUsername)
        {
            var messageSets = await _messagesRepository.Get(followeePartialUsername, false);
            Feed feed = new Feed();
            feed.Add(messageSets);
            return feed;
        }

        public async Task<Feed> GetUserFeed(string currentUsername, string followeePartialUsername)
        {
            var followees = await _followersRepository.Get(currentUsername);
            var relevantFollowees = followees.Where(f => f.Contains(followeePartialUsername)).ToArray();
            var messageSets = await _messagesRepository.Get(relevantFollowees, true);
            var feed = new Feed();
            feed.Add(messageSets);
            return feed;
        }
    }
}
