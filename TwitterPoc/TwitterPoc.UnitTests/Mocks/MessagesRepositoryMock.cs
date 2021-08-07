using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Data.Interfaces;

namespace TwitterPoc.UnitTests.Mocks
{
    public class MessagesRepositoryMock : IMessagesRepository
    {
        private readonly List<Message> _messages = new List<Message>();

        public async Task Add(Message message)
        {
            await Task.FromResult(0);

            _messages.Add(message);
        }

        public async Task<IEnumerable<Message>> Get(string username, bool exactMatch, int limit)
        {
            await Task.FromResult(0);

            if (exactMatch)
            {
                return _messages.Where(m => m.Username == username).Take(limit);
            }
            else
            {
                return _messages.Where(m => m.Username.Contains(username)).Take(limit);
            }
        }

        public async Task<IEnumerable<Message>> Get(IEnumerable<string> usernames, bool exactMatch, int limit)
        {
            await Task.FromResult(0);
            var messages = new List<Message>();
            foreach (var username in usernames)
            {
                messages.AddRange(await Get(username, exactMatch, limit));
            }

            return messages;
        }
    }
}
