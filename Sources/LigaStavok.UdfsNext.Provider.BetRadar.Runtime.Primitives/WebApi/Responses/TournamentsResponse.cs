using System.Collections.ObjectModel;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public class TournamentsResponse : Collection<Tournament>, IWebApiResponse
	{
	}
}
