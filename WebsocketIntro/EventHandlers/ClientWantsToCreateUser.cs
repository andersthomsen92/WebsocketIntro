using Fleck;
using WebSocketBoilerplate;

namespace WebsocketIntro.EventHandlers;

public class ClientWantsToCreateUser : BaseDto
{
    public required string Name { get; set; }
    public string requestId { get; set; } 
}

public class ServerConfirmUserCreated : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string requestId { get; set; }
}


public class CreateUserHandler(ApplicationDbContext dbContext, ClientWantsToCreateUserValidator validator) : BaseEventHandler<ClientWantsToCreateUser>
{
    public override async Task Handle(ClientWantsToCreateUser dto, IWebSocketConnection socket)
    {

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            // If validation fails, send an error message to the client
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            socket.SendDto(new ServerSendsErrorMessageDto
            {
                Error = errorMessage // Combine all validation errors into a single string
            });
            return; 
        }
        try
        {
            var user = new User { name = dto.Name };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            
            socket.SendDto(new ServerConfirmUserCreated 
            { 
                Id = user.id,
                Name = user.name,
                requestId = dto.requestId
            });
        }
        catch (Exception ex)
        {
            socket.SendDto(new ServerSendsErrorMessageDto
            {
                Error = "An error occurred while creating the user. Please try again later."
            });
        }
    }
}
