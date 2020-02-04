using Orleans;

namespace LigaStavok.UdfsNext.Clustering.Client
{
	internal interface IClusterClientBuilder<TCluster>
	{
		IClusterClient Build();
    }
}