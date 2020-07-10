using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.Processors
{
	public class ProcessorOptions
	{
		public bool Reentrancable { get; set; } = false;

		public int ThreadCount { get; set; } = 1;
	}
}
