using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Providers
{
	public interface ITranslationDistributer
	{
		Task Distribute(long translationId);
	}
}
