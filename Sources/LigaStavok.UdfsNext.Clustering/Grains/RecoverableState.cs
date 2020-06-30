using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Orleans.Grains
{
	public class RecoverableState : IRecoverableState
	{
		public bool Saved { get; set; }
	}
}
