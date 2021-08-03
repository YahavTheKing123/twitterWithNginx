using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace TwitterPoc.Models
{
    [DataContract]
    public class ErrorModel
    {
        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
