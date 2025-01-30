namespace WebsocketIntro;
using Fleck;

public class ClientConnectionsState
{
    public List<IWebSocketConnection> ClientConnections { get; set; } = new List<IWebSocketConnection>() { };
}