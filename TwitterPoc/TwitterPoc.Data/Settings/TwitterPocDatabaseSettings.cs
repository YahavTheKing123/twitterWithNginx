using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoc.Data.Settings
{
    public class TwitterPocDatabaseSettings : ITwitterPocDatabaseSettings
    { 
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITwitterPocDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
