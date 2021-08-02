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
            var messageToAdd = new Message() {
                Content = message,
                Username = username,
                Time = DateTime.UtcNow,
            };
            await _messagesRepository.Add(messageToAdd);
        }

        public async Task<Feed> GetGlobalFeed(string followeePartialUsername)
        {
            var messages = await _messagesRepository.Get(followeePartialUsername);
            Feed feed = new Feed()
            {
                Messages = messages.ToList()
            };
            return feed;
        }

        public async Task<Feed> GetUserFeed(string currentUsername, string followeePartialUsername)
        {
            var followees = await _followersRepository.Get(currentUsername);
            var messages = await _messagesRepository.Get(followees);
            return new Feed() { 
                Messages = messages.ToList()
            };
        }
    }
}
