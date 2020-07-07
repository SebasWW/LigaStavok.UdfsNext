using Orleans;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public interface IUdfsClusterClientBuilder
	{
		IUdfsClusterClient Build(UdfsClusterClientOptions options);
    }
}