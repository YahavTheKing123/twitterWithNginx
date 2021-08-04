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
        private readonly Dictionary<string, MessagesSet> _messages = new Dictionary<string, MessagesSet>();

        public async Task Add(string username, Message message)
        {
            await Task.FromResult(0);

            if (_messages.ContainsKey(username))
            {
                _messages[username].Messages.Add(message);
            }
        }

        public async Task Add(string username, bool ignoreKeyDuplication)
        {
            await Task.FromResult(0);
            if (ignoreKeyDuplication && _messages.ContainsKey(username))
            {
                return;
            }
            _messages.Add(username, new MessagesSet() { Username = username, Messages = new List<Message>() });
        }

        public async Task<IEnumerable<MessagesSet>> Get(string username, bool exactMatch)
        {
            await Task.FromResult(0);

            if (exactMatch)
            {
                if (_messages.ContainsKey(username))
                {
                    return new[] { _messages[username] };
                }
            }
            else
            {
                return _messages.Where(kvp => kvp.Key.Contains(username)).Select(kvp => kvp.Value);
            }

            return Enumerable.Empty<MessagesSet>();
        }

        public async Task<IEnumerable<MessagesSet>> Get(IEnumerable<string> usernames, bool exactMatch)
        {
            await Task.FromResult(0);
            var messages = new List<MessagesSet>();
            foreach (var username in usernames)
            {
                messages.AddRange(await Get(username, exactMatch));
            }

            return messages;
        }
    }
}
