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
        private readonly Dictionary<string, List<Message>> _messages = new Dictionary<string, List<Message>>();

        public async Task Add(Message message)
        {

            await Task.FromResult(0);

            var username = message.Username;
            if (!_messages.ContainsKey(username))
            {
                _messages.Add(username, new List<Message>());
            }
            _messages[username].Add(message);
        }

        public async Task<IEnumerable<Message>> Get(string username)
        {
            await Task.FromResult(0);

            if (_messages.ContainsKey(username))
            {
                return _messages[username];
            }
            return Enumerable.Empty<Message>();
        }

        public async Task<IEnumerable<Message>> Get(IEnumerable<string> usernames)
        {
            await Task.FromResult(0);
            var messages = new List<Message>();
            foreach (var username in usernames)
            {
                if (_messages.ContainsKey(username))
                {
                    messages.AddRange(_messages[username]);
                }
            }

            return messages;
        }

    }
}
