using System;
using System.Runtime.Caching;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
    public sealed class ApiCache : IApiCache
    {
        private readonly ObjectCache _cache;
        private static object _sync;

        public ApiCache()
        {
            _cache = MemoryCache.Default;
            _sync = new object();
        }

        public void Add(ApiCommand message)
        {
            lock (_sync)
            {
                _cache.Set(message.Endpoint.ToString(), Guid.NewGuid(), DateTimeOffset.UtcNow.Add(message.CachePeriod));
            }
        }

        public bool Contains(ApiCommand message)
        {
            lock (_sync)
            {
                return _cache.Contains(message.Endpoint.ToString());
            }
        }
    }
}