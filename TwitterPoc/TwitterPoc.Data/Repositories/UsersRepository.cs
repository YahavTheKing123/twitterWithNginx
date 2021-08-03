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
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly IMongoCollection<User> _users;

        public UsersRepository(ITwitterPocDatabaseSettings settings, ILogger<UsersRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>("users");
        }
        public async Task<User> GetAsync(string username)
        {
            try
            {
                var result = await _users.FindAsync(u => u.Username == username);
                return await result.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Error on GetAsync. Username: {username}");
                throw;
            }

        }

        public async Task AddAsync(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Error on AddAsync. Username: {user?.Username}");
                throw;
            }
        }
    }
}
