using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Data.Entities
{
    public class UserFollowees
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        public List<string> Followees { get; set; }
    }
}
