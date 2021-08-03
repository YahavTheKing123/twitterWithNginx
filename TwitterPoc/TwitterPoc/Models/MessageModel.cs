using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TwitterPoc.Models
{
    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }

        public MessageModel() { }

        public MessageModel(string username, string content, DateTime time)
        {
            Username = username;
            Content = content;
            Time = time;
        }
    }
}
