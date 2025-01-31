using Fleck;

namespace WebsocketIntro.EventHandlers;

public class ClientConnectionsState
{
    public List<IWebSocketConnection> ClientConnections { get; set; } = new List<IWebSocketConnection>() { };
}