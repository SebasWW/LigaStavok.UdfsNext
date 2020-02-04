using System;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineProviderGrainService
	{
		//void Initialize(UdfsNextProviderOptions options);

		string Id { get; }

		string Name { get; }
	}
}
