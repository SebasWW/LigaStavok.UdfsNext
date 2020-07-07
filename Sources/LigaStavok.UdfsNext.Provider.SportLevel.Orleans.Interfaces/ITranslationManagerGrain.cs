using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public interface ITranslationManagerGrain : IGrainWithIntegerKey
	{
		Task InitializeAsync(MessageContext messageContext);
	}
}
