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
        public IEnumerable<Message> Messages { get; set; }

        public Feed()
        {
        }
    }
}
