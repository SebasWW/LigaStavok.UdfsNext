using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public class Lang
	{
		private Lang() { }

		public string Code { get; private set; }

		public static Lang FromCode(string code)
		{
			return new Lang() { Code = code };
		}
	}
}
