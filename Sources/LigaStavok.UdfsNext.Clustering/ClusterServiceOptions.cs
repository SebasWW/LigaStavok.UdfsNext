﻿namespace LigaStavok.UdfsNext.Clustering
{
    public class ClusterServiceOptions : IClusterServiceOptions
    {
        /// <summary>
        /// Gets or sets the cluster identity.
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier for this service, which should survive deployment and redeployment
        /// </summary>
        public string ServiceId { get; set; }
    }
}
