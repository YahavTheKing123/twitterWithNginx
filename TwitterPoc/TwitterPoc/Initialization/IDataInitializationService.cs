using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Logic.Interfaces;

namespace TwitterPoc.Initialization
{
    public interface IDataInitializationService
    {
        Task AddSampleDataIfEmptyProject();
    }
}
