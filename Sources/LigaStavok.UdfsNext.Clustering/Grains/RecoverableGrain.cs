using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Clustering.Grains
{
	public abstract class RecoverableGrain<TState> : Grain
		where TState : IRecoverableState, new()
	{
		private readonly IPersistentState<TState> state;

		// [PersistentState("saved", UdfsGrainStorages.UDFS_GRAIN_STORAGE)] 
		public RecoverableGrain(IPersistentState<TState> state) 
		{
			this.state = state;
		}

		protected TState State { get => state.State; }

		public abstract Task OnRecoveryAsync();

		public override async Task OnActivateAsync()
		{
			if (!state.State.Saved) await OnRecoveryAsync();

			state.State.Saved = false;
			await state.WriteStateAsync();
		}

		public override async Task OnDeactivateAsync()
		{
			state.State.Saved = true;
			await state.WriteStateAsync();
		}
	}
}
