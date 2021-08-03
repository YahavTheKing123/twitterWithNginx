using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using TwitterPoc.Data.Entities;
using TwitterPoc.Logic.Entities;

namespace TwitterPoc.Models
{
    [DataContract]
    public class FeedModel
    {
        [DataMember(Name = "messages")]
        public IEnumerable<MessageModel> Messages { get; set; }

        public FeedModel()
        { 
        }

        public FeedModel(Feed feed)
        {
            feed.Messages.Select(m => new MessageModel(m.Username, m.Content, m.Time));
        }
    }
}
