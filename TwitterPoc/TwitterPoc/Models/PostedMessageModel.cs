using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterPoc.Models
{
    public class PostedMessageModel
    {
        [Required]
        [StringLength(140)]
        public string MessageBody { get; set; }
    }
}
