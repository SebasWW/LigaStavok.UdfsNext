using System.Threading.Tasks;
using Orleans;

namespace LigaStavok.UdfsNext.HealthCheck.Clustering.Grains
{
    public interface ILocalHealthCheckGrain : IGrainWithGuidKey
    {
        Task PingAsync();
    }
}
