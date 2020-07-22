using System.ComponentModel;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public class ApiCommandRequest : IApiCommandCreateRequest
	{

		public string EventId { get; set; }

		public ProductType ProductType { get; set; }
	}
}
