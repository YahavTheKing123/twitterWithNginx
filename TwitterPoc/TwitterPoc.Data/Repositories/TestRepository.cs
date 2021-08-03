using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Settings;

namespace TwitterPoc.Data.Repositories
{
    public class TestRepository
    {
        private readonly IMongoCollection<User> _members;

        public TestRepository(ITwitterPocDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _members = database.GetCollection<User>("users");
        }

        public async Task<List<User>> GetAsync()
        {
            var result = await _members.FindAsync<User>(user => true);
            return result.ToList();
        }

        public async Task<User> GetAsync(string username)
        {
            var result = await _members.FindAsync<User>(user => user.Username == username);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(User book)
        {
            await _members.InsertOneAsync(book);

            return book;
        }
    }
}
