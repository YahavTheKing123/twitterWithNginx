using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;

namespace TwitterPoc.Logic.Entities
{
    public class Feed
    {
        public List<Message> Messages { get; set; }

        public Feed()
        {
            Messages = new List<Message>();
        }

        public void Add(IEnumerable<MessagesSet> messageSets)
        {
            foreach (var set in messageSets)
            {
                var username = set.Username;
                var messagesToAdd = set.Messages.Select(m => new Message(username, m.Content, m.Time));
                this.Messages.AddRange(messagesToAdd);
            }
        }
    }
}
