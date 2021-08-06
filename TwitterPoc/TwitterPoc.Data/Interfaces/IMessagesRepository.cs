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
        Task Add(Message message);
        Task<IEnumerable<Message>> Get(string username, bool exactMatch);
        Task<IEnumerable<Message>> Get(IEnumerable<string> usernames, bool exactMatch);
    }
}
