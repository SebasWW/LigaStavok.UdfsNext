using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Line
{
	interface IUdfsNextBettor
	{
		Task SetBetAsync(string eventId);
	}
}
