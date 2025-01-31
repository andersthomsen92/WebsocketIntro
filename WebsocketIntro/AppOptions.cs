namespace WebsocketIntro;

public class AppOptions
{
    public string JwtSecret { get; set; } = default!;
    
    public required string ConnectionString { get; set; }
    
}