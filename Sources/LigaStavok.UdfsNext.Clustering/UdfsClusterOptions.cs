using System.Collections.Generic;
using System.Reflection;
using Orleans.Configuration;

namespace LigaStavok.UdfsNext.Orleans
{
	public class UdfsClusterOptions
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
        /// The persistent storage configuration.
        /// </summary>
        public DbConnectionOptions Storage { get; set; } = new DbConnectionOptions();

        /// <summary>
        /// The persistent reminder configuration.
        /// </summary>
        public DbConnectionOptions Reminder { get; set; } = new DbConnectionOptions();
    }
}
