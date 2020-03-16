using System.Net;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Line;
using LigaStavok.UdfsNext.Line.Grains;

namespace LigaStavok.UdfsNext.Clustering
{
	public static class UdfsClusterOptionsExtension
	{
		public static void Configure(this UdfsClusterOptions options, ClusterConfiguration configuration)
		{
			// Clustering information
			options.ClusterService.ClusterId = configuration.ClusterId;
			options.ClusterService.ServiceId = configuration.ServiceId;

			// Clustering provider
			options.Membership.Enabled = configuration.Membership.Enabled;
			options.Membership.ConnectionString = configuration.Membership.ConnectionString;
			options.Membership.Provider = configuration.Membership.Provider;

			options.Reminder.Enabled = configuration.Reminder.Enabled;
			options.Reminder.ConnectionString = configuration.Reminder.ConnectionString;
			options.Reminder.Provider = configuration.Reminder.Provider;

			options.Storage.Enabled = configuration.Storage.Enabled;
			options.Storage.ConnectionString = configuration.Storage.ConnectionString;
			options.Storage.Provider = configuration.Storage.Provider;

			// Port to use for the gateway
			options.EndPoint.GatewayPort = configuration.Endpoint.GatewayPort;

			// Port to use for Silo-to-Silo
			options.EndPoint.SiloPort = configuration.Endpoint.SiloPort;

			// IP Address to advertise in the cluster
			if (IPAddress.TryParse(configuration.Endpoint.AdvertisedIPAddress, out var iPAddress)) options.EndPoint.AdvertisedIPAddress = iPAddress;

			// The socket used for silo-to-silo will bind to this endpoint
			if (configuration.Endpoint.GatewayListeningPort > 0 && IPAddress.TryParse(configuration.Endpoint.AdvertisedIPAddress, out var gatewayIPAddress))
					options.EndPoint.GatewayListeningEndpoint = new IPEndPoint(gatewayIPAddress, configuration.Endpoint.GatewayListeningPort);

			// The socket used by the gateway will bind to this endpoint
			if (configuration.Endpoint.SiloListeningPort > 0 && IPAddress.TryParse(configuration.Endpoint.SiloListeningIP, out var siloIPAddress))
				options.EndPoint.SiloListeningEndpoint = new IPEndPoint(siloIPAddress, configuration.Endpoint.SiloListeningPort);
		}
	}
}
