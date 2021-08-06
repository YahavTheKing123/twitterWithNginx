using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly int _maxMessagesPerFeed;

        public FeedsService(IMessagesRepository messagesRepository, IFollowersRepository followersRepository, IConfiguration config, ILogger<FeedsService> logger)
        {
            _messagesRepository = messagesRepository;
            _followersRepository = followersRepository;
            _logger = logger;

            if (int.TryParse(config["MaxMessagesPerFeed"], out int maxMessagesPerFeed))
            {
                _maxMessagesPerFeed = maxMessagesPerFeed;
                _logger.LogError($"'MaxMessagesPerFeed' parameter has been parsed successfully and its value is: {_maxMessagesPerFeed}");
            }
            else
            {
                _maxMessagesPerFeed = 50;
                _logger.LogError($"Could find 'MaxMessagesPerFeed' in configuration. Using {_maxMessagesPerFeed} as default.");
            }
        }

        public async Task AddMessage(string username, string message)
        {
            var messageToAdd = new Data.Entities.Message() {
                Username = username,
                Content = message,
                Time = DateTime.UtcNow,
            };
            await _messagesRepository.Add(messageToAdd);
        }

        public async Task<Feed> GetGlobalFeed(string followeePartialUsername)
        {
            var feed = new Feed();
            var messages = await _messagesRepository.Get(followeePartialUsername, false, _maxMessagesPerFeed);
            feed.Add(messages);
            return feed;
        }

        public async Task<Feed> GetUserFeed(string currentUsername, string followeePartialUsername)
        {
            Feed feed = new Feed();

            if (string.IsNullOrEmpty(followeePartialUsername))
            {
                _logger.LogInformation($"A request to get user feed with an empty followee name. currentUsername={currentUsername}");

                return feed;
            }
            var followees = await _followersRepository.Get(currentUsername);
            var relevantFollowees = followees.Where(f => f.Contains(followeePartialUsername)).ToArray();
            var messageSets = await _messagesRepository.Get(relevantFollowees, true, _maxMessagesPerFeed);
            feed.Add(messageSets);
            return feed;
        }
    }
}
