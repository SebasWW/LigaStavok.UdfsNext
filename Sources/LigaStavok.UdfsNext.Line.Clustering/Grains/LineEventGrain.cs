using System.Threading.Tasks;
using LigaStavok.UdfsNext.Clustering;
using LigaStavok.UdfsNext.Clustering.Grains;
using LigaStavok.UdfsNext.Remoting;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Line.Grains
{
	public class LineEventGrain : RecoverableGrain<LineEventGrainState>, IUdfsLineEventGrain
	{
		private LineEvent model;
		private bool isRecovered = false;

		public LineEventGrain([PersistentState("saved", UdfsGrainStorages.UDFS_GRAIN_STORAGE)] IPersistentState<LineEventGrainState> state) : base(state){}

		public override async Task OnActivateAsync()
		{
			await base.OnActivateAsync();
			if (!isRecovered) await LoadEventAsync();
		}

		public override async Task OnDeactivateAsync()
		{
			await SaveEventAsync();
			await base.OnDeactivateAsync();
		}

		public override async Task OnRecoveryAsync()
		{
			isRecovered = true;
			throw new System.NotImplementedException();
		}

		private async Task SaveEventAsync()
		{
			// saving model
		}

		private async Task LoadEventAsync()
		{
			// loading model
			model = new LineEvent();
		}

		public async Task SaveAsync()
		{
			await SaveEventAsync();
		}
		public async Task<UdfsResponse<LineEvent>> GetAsync(UdfsRequest request)
		{
			return request.ToResponse(model);
		}
	}
}
