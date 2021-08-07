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
        public List<string> Followees { get; set; }

        public Feed()
        {
            Messages = new List<Message>();
            Followees = new List<string>();
        }

        public void Add(IEnumerable<Data.Entities.Message> messages)
        {
            var messagesToAdd = messages.Select(m => new Message(m.Username, m.Content, m.Time));
            this.Messages.AddRange(messagesToAdd);
        }
    }
}
