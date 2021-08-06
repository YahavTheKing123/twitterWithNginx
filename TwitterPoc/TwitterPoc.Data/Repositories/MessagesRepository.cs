﻿using Microsoft.Extensions.Logging;
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
        private readonly IMongoCollection<Message> _messages;
        private readonly ILogger<MessagesRepository> _logger;

        public MessagesRepository(ITwitterPocDatabaseSettings settings, ILogger<MessagesRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<Message>("messages");
        }


        public async Task Add(Message message)
        {
            try
            {
                await _messages.InsertOneAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on adding a message. Username: {message.Username}. Time of message: {message?.Time}. Content: {message?.Content}");
                throw;
            }

        }


        public async Task<IEnumerable<Message>> Get(string username, bool exactMatch)
        {
            try
            {
                IAsyncCursor<Message> result;
                var options = GetDescendingSortOptions<Message>("Time");
                if (exactMatch)
                {
                    result = await _messages.FindAsync(m => m.Username == username, options);
                }
                else
                {
                    result = await _messages.FindAsync(m => m.Username.Contains(username), options);
                }

                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on getting messages. Username: {username}.");
                throw;
            }

        }

        public async Task<IEnumerable<Message>> Get(IEnumerable<string> usernames, bool exactMatch)
        {
            try
            {
                var usernamesHashSet = usernames.ToHashSet();
                IAsyncCursor<Message> result;
                var options = GetDescendingSortOptions<Message>("Time");

                if (exactMatch)
                {
                    result = await _messages.FindAsync(m => usernamesHashSet.Contains(m.Username), options);
                }
                else
                {
                    result = await _messages.FindAsync(m => usernamesHashSet.Any(username => m.Username.Contains(username)), options);
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

        private FindOptions<T> GetDescendingSortOptions<T>(string field)
        {
            return new FindOptions<T>
            {
                Sort = Builders<T>.Sort.Descending(field)
            };
        }

    }
}
