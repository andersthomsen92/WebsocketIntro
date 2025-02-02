using Fleck;
using WebSocketBoilerplate;

namespace WebsocketIntro.EventHandlers;


public class ServerConfirmsUserDeleted : BaseDto
{
    public int id { get; set; }
    public string requestId { get; set; }
}


public class ClientWantsToDeleteUser : BaseDto
{
    public int id { get; set; }
    public string requestId { get; set; }
}

public class DeleteUserHandler(ApplicationDbContext dbContext) : BaseEventHandler<ClientWantsToDeleteUser>
{
    public override async Task Handle(ClientWantsToDeleteUser dto, IWebSocketConnection socket)
    {
        var user = await dbContext.Users.FindAsync(dto.id);
        if (user == null)
        {
            socket.SendDto(new ServerSendsErrorMessageDto { Error = "User not found." });
            return;
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();

        socket.SendDto(new ServerConfirmsUserDeleted
        {
            id = user.id,
            requestId = dto.requestId
        });
    }
}