namespace LigaStavok.UdfsNext.Configuration
{

	public class EndpointConfiguration
	{
		public const int DEFAULT_SILO_PORT = 11111;
		public const int DEFAULT_GATEWAY_PORT = 30000;

		/// <summary>
		/// The IP address used for Orleans.
		/// </summary>
		public string AdvertisedIPAddress { get; set; }

		/// <summary>
		/// The port this silo uses for silo-to-silo communication.
		/// </summary>
		public int SiloPort { get; set; } = DEFAULT_SILO_PORT;

		/// <summary>
		/// The port this silo uses for silo-to-client (gateway) communication. Specify 0 to disable gateway functionality.
		/// </summary>
		public int GatewayPort { get; set; } = DEFAULT_GATEWAY_PORT;

		/// <summary>
		/// The IP address used to listen for silo to silo communication. 
		/// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="SiloPort"/>
		/// </summary>
		public string SiloListeningIP { get; set; }

		/// <summary>
		/// The port used to listen for silo to silo communication. 
		/// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="SiloPort"/>
		/// </summary>
		public int SiloListeningPort { get; set; }

		/// <summary>
		/// The IP address used to listen for silo to silo communication. 
		/// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="GatewayPort"/>
		/// </summary>
		public string GatewayListeningIP { get; set; }

		/// <summary>
		/// The port used to listen for silo to silo communication. 
		/// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="GatewayPort"/>
		/// </summary>
		public int GatewayListeningPort { get; set; }
	}
}
