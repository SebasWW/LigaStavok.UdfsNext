namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket
{
	public interface IWebSocketMessageParser
	{
		object Parse(string text);
	}
}