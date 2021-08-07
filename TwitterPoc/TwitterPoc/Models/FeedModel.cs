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

        [DataMember(Name = "followees")]
        public IEnumerable<string> Followees { get; set; }

        public FeedModel()
        { 
        }

        public FeedModel(Feed feed)
        {
            Messages = feed.Messages.Select(m => new MessageModel(m.Username, m.Content, m.Time));
            Followees = feed.Followees;
        }
    }
}
