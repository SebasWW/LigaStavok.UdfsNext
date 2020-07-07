using System.Collections.ObjectModel;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses
{
	public class TournamentsResponse : Collection<Tournament>, IWebApiResponse
	{
	}
}
