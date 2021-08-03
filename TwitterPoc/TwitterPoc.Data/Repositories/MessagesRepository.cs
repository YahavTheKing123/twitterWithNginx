using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Data.Settings;

namespace TwitterPoc.Data.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly IMongoCollection<MessagesSet> _messages;
        private readonly ILogger<MessagesRepository> _logger;

        public MessagesRepository(ITwitterPocDatabaseSettings settings, ILogger<MessagesRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<MessagesSet>("messages");
        }

        public async Task Add(string username, Message message)
        {
            try
            {
                var filter = Builders<MessagesSet>.Filter.Eq(e => e.Username, username);

                var update = Builders<MessagesSet>.Update.Push(e => e.Messages, message);

                await _messages.FindOneAndUpdateAsync(filter, update);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on adding a message. Username: {username}. Time of message: {message?.Time}. Content: {message?.Content}");
                throw;
            }

        }

        public async Task<IEnumerable<MessagesSet>> Get(string username, bool exactMatch)
        {
            try
            {
                IAsyncCursor<MessagesSet> result;
                if (exactMatch)
                {
                    result = await _messages.FindAsync(m => m.Username == username);
                }
                else
                {
                    result = await _messages.FindAsync(m => m.Username.Contains(username));
                }
                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on getting messages. Username: {username}.");
                throw;
            }

        }

        public async Task<IEnumerable<MessagesSet>> Get(IEnumerable<string> usernames, bool exactMatch)
        {
            try
            {
                var usernamesHashSet = usernames.ToHashSet();
                IAsyncCursor<MessagesSet> result;
                if (exactMatch)
                {
                    result = await _messages.FindAsync(m => usernamesHashSet.Contains(m.Username));
                }
                else
                {
                    result = await _messages.FindAsync(m => usernamesHashSet.Any(username => m.Username.Contains(username)));
                }

                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                var usernamesAsString = string.Join(", ", usernames == null ? new string[0] : usernames.ToArray());
                _logger.LogError(e, $"Error on getting messages. Usernames: {usernamesAsString}.");
                throw;
            }

        }

    }
}
