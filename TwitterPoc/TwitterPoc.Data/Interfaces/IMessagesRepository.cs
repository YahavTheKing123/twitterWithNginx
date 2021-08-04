using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;

namespace TwitterPoc.Data.Interfaces
{
    public interface IMessagesRepository
    {
        Task Add(string username, bool ignoreKeyDuplication);
        Task Add(string username, Message message);
        Task<IEnumerable<MessagesSet>> Get(string username, bool exactMatch);
        Task<IEnumerable<MessagesSet>> Get(IEnumerable<string> usernames, bool exactMatch);
    }
}
