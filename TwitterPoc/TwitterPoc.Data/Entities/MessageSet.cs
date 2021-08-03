using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Data.Entities
{
    public class Message
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }

    public class MessagesSet
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        public List<Message> Messages { get; set; }
    }
}
