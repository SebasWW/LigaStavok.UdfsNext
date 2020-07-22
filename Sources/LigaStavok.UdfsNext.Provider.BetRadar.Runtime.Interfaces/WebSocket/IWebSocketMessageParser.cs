namespace LigaStavok.UdfsNext.Provider.BetRadar.WebSocket
{
	public interface IWebSocketMessageParser
	{
		object Parse(string text);
	}
}