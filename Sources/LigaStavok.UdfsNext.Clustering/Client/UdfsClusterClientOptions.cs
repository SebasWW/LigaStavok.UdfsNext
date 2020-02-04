using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Orleans.Configuration;

namespace LigaStavok.UdfsNext.Clustering.Client
{
    /// <summary>
    /// The cluster client builder configuration information.
    /// </summary>
    public class UdfsClusterClientOptions<TCluster> 
    //    : UdfsClusterClientOptions
    //{

    //}
    //public class UdfsClusterClientOptions
    {
        /// <summary>
        /// The cluster configuration information.
        /// </summary>
        public ClusterServiceOptions ClusterService { get; set; } = new ClusterServiceOptions();

        /// <summary>
        /// The cluster endpoint information.
        /// </summary>
        public EndpointOptions EndPoint { get; set; } = new EndpointOptions();

        /// <summary>
        /// Configuration to membership storage.
        /// </summary>
        public DbConnectionOptions Membership { get; set; } = new DbConnectionOptions();

        /// <summary>
        /// Assemblies with grains interfaces
        /// </summary>
        public List<Assembly> GrainAssemblies { get; } = new List<Assembly>();

        public int ConnectionRetryCount { get; set; } 
    }
}
