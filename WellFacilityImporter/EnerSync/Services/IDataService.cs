using EnerSync.Data;

namespace EnerSync.Services
{
    public interface IDataService
    {
        EnerSyncContext Context { get; set; }
    }
}
