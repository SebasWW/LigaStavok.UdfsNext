using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface ITranslationDistributer
	{
		Task Distribute(MessageContext<Translation> messageContext);
	}
}
