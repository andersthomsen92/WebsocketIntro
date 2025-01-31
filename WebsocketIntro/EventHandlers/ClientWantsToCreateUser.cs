using Fleck;
using WebSocketBoilerplate;

namespace WebsocketIntro.EventHandlers;

public class ClientWantsToCreateUser : BaseDto
{
    public string Name { get; set; }
}

public class ServerConfirmUserCreated : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CreateUserHandler(ApplicationDbContext dbContext) : BaseEventHandler<ClientWantsToCreateUser>
{
    public override async Task Handle(ClientWantsToCreateUser dto, IWebSocketConnection socket)
    {
        var user = new User { name = dto.Name };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        socket.SendDto(new ServerConfirmUserCreated { Id = user.id, Name = user.name });
    }
}