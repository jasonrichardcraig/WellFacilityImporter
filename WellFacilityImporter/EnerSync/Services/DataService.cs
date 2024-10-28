using EnerSync.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnerSync.Services
{
    public class DataService : IDataService
    {
        public EnerSyncContext Context { get; set; }

        public DataService(EnerSyncContext enerSyncContext)
        {
            Context = enerSyncContext;
        }
 
    }
}
