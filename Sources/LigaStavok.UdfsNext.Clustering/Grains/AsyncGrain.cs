using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Orleans.Grains
{
	public abstract class AsyncGrain : RecoverableGrain<RecoverableState>
	{
		public AsyncGrain(IPersistentState<RecoverableState> state) : base(state){}

		protected IDisposable AddTask(Func<object,Task> func, object objectState)
		{
			return RegisterTimer(func, objectState, TimeSpan.Zero, TimeSpan.Zero);
		}
	}
}
