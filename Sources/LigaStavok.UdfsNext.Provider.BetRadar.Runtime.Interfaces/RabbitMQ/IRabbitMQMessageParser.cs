namespace LigaStavok.UdfsNext.Provider.BetRadar.WebSocket
{
	public interface IRabbitMQMessageParser
	{
		object Parse(string text);
	}
}