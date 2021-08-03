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
    public class FollowersRepository : IFollowersRepository
    {
        
        private readonly IMongoCollection<UserFollowees> _followers;
        private readonly ILogger<FollowersRepository> _logger;

        public FollowersRepository(ITwitterPocDatabaseSettings settings, ILogger<FollowersRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _followers = database.GetCollection<UserFollowees>("followers");
        }
        
        

        public async Task Add(string follower, string followee)
        {
            try
            {
                var filter = Builders<UserFollowees>.Filter.Eq(e => e.Username, follower);

                var update = Builders<UserFollowees>.Update.Push(e => e.Followees, followee);

                await _followers.FindOneAndUpdateAsync(filter, update);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on adding a followee. Follower: {follower}, Follower: {followee}.");
                throw;
            }
        }

        public async Task Remove(string follower, string followee)
        {
            try
            {
                var filter = Builders<UserFollowees>.Filter.Eq(e => e.Username, follower);

                var update = Builders<UserFollowees>.Update.Pull(e => e.Followees, followee);

                await _followers.FindOneAndUpdateAsync(filter, update);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on removing a followee. Follower: {follower}, Follower: {followee}.");
                throw;
            }
        }

        public async Task<IEnumerable<string>> Get(string followerUsername)
        {
            try
            {
                var result = await _followers.FindAsync(f => f.Username == followerUsername);
                var userFollowees = await result.FirstOrDefaultAsync();
                return userFollowees.Followees;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on getting followees. Follower: {followerUsername}");
                throw;
            }
        }
    }
}
