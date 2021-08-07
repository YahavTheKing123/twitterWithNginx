using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.UnitTests.Mocks
{
    public class ConfigurationMock : IConfiguration
    {
        public string this[string key] 
        {
            get
            {
                if (key == "MaxMessagesPerFeed")
                {
                    return "10";
                }
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
