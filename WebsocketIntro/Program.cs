using System.Reflection;
using Fleck;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebSocketBoilerplate;
using WebsocketIntro;
using WebsocketIntro.EventHandlers;
using FluentValidation.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(nameof(AppOptions)));

// Register EF Core DbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var appOptions = serviceProvider.GetRequiredService<IOptions<AppOptions>>().Value;
    options.UseNpgsql(appOptions.ConnectionString);
});

// Register services
builder.Services.AddSingleton<ClientConnectionsState>();
builder.Services.AddSingleton<SecurityService.SecurityService>();



// Inject WebSocket event handlers
builder.Services.InjectEventHandlers(Assembly.GetExecutingAssembly());
// Register FluentValidation validators in the container
builder.Services.AddValidatorsFromAssemblyContaining<ClientWantsToCreateUserValidator>();




var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Run database migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

var server = new WebSocketServer("ws://0.0.0.0:8181");

var clientConnections = app.Services.GetRequiredService<ClientConnectionsState>().ClientConnections;
server.Start(socket =>
{
    socket.OnOpen = () => clientConnections.Add(socket);
    socket.OnClose = () => clientConnections.Remove(socket);
    socket.OnMessage = message =>
    {
        Task.Run(async () =>
        {
            try
            {
                await app.CallEventHandler(socket, message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error handling message: {Error}", e.Message);
                socket.SendDto(new ServerSendsErrorMessageDto { Error = e.Message });
            }
        });
    };
});

app.Run();