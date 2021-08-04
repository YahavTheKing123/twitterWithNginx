using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterPoc.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} must have between 3 to 50 characters.", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long, but less than {1}.", MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
