using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.HealthCheck.Orleans.Grains;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orleans;

namespace LigaStavok.UdfsNext.HealthCheck.Orleans
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IClusterClient client;

        public StorageHealthCheck(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await client.GetGrain<IStorageHealthCheckGrain>(Guid.NewGuid()).CheckAsync();
            }
            catch (Exception error)
            {
                return HealthCheckResult.Unhealthy("Failed to ping the storage health check grain.", error);
            }
            return HealthCheckResult.Healthy();
        }
    }
}
