namespace LigaStavok.UdfsNext.Configuration
{
    /// <summary>
    /// The cluster configuration information.
    /// </summary>
	public class OrleansClusterConfiguration
	{
        /// <summary>
        /// Gets or sets the cluster identity.
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier for this service, which should survive deployment and redeployment
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// The cluster endpoint information.
        /// </summary>
        public EndpointConfiguration Endpoint { get; set; } = new EndpointConfiguration();

        /// <summary>
        /// Configuration to membership storage.
        /// </summary>
        public ConnectionConfiguration Membership { get; set; } = new ConnectionConfiguration();

        /// <summary>
        /// The persistent storage configuration.
        /// </summary>
        public ConnectionConfiguration Storage { get; set; } = new ConnectionConfiguration();

        /// <summary>
        /// The persistent reminder configuration.
        /// </summary>
        public ConnectionConfiguration Reminder{ get; set; } = new ConnectionConfiguration();
    }
}