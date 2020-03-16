using System.Threading.Tasks;
using Orleans;

namespace LigaStavok.UdfsNext.HealthCheck.Orleans.Grains
{
    public interface IStorageHealthCheckGrain : IGrainWithGuidKey
    {
        Task CheckAsync();
    }
}
