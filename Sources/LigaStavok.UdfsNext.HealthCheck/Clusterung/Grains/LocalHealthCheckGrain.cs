using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace LigaStavok.UdfsNext.HealthCheck.Orleans.Grains
{
    [StatelessWorker(1)]
    public class LocalHealthCheckGrain : Grain, ILocalHealthCheckGrain
    {
        public Task PingAsync() => Task.CompletedTask;
    }
}
