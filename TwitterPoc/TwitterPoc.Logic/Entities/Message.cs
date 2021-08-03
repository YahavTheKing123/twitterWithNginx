using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Logic.Entities
{
    public class Message
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public Message(string username, string content, DateTime time)
        {
            Username = username;
            Content = content;
            Time = time;
        }
    }
}
