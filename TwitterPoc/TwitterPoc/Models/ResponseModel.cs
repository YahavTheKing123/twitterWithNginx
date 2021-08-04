using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TwitterPoc.Models
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public ResponseModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
