using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Data.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public string Username { get; }
        public UsernameAlreadyExistsException(string username)
        {
            Username = username;
        }
    }
}
