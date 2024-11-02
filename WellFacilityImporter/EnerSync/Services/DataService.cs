using EnerSync.Data;

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
