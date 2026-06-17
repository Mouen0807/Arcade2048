namespace Arcade2048.Resources;

public class Arcade2048Options
{
    public const string SectionName = "Arcade2048";
    
    public RabbitMqOptions RabbitMq { get; set; } = new RabbitMqOptions();
    public ConnectionStringOptions ConnectionStrings { get; set; } = new ConnectionStringOptions();
    public AuthOptions Auth { get; set; } = new AuthOptions();
    public string JaegerHost { get; set; } = String.Empty;
    
    public class RabbitMqOptions
    {
        public const string SectionName = $"{Arcade2048Options.SectionName}:RabbitMq";
        public const string HostKey = nameof(Host);
        public const string VirtualHostKey = nameof(VirtualHost);
        public const string UsernameKey = nameof(Username);
        public const string PasswordKey = nameof(Password);
        public const string PortKey = nameof(Port);

        public string Host { get; set; } = String.Empty;
        public string VirtualHost { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Port { get; set; } = String.Empty;
    }

    public class ConnectionStringOptions
    {
        public const string SectionName = $"{Arcade2048Options.SectionName}:ConnectionStrings";
        public const string Arcade2048Key = nameof(Arcade2048); 
            
        public string Arcade2048 { get; set; } = String.Empty;
    }
    
    
    public class AuthOptions
    {
        public const string SectionName = $"{Arcade2048Options.SectionName}:Auth";

        public string Audience { get; set; } = String.Empty;
        public string Authority { get; set; } = String.Empty;
        public string AuthorizationUrl { get; set; } = String.Empty;
        public string TokenUrl { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecret { get; set; } = String.Empty;
    }
}

public static class Arcade2048OptionsExtensions
{
    public static Arcade2048Options GetArcade2048Options(this IConfiguration configuration)
    {
        return configuration
            .GetSection(Arcade2048Options.SectionName)
            .Get<Arcade2048Options>();
    }
    
    public static Arcade2048Options.RabbitMqOptions GetRabbitMqOptions(this IConfiguration configuration)
    {
        return configuration
            .GetSection(Arcade2048Options.RabbitMqOptions.SectionName)
            .Get<Arcade2048Options.RabbitMqOptions>();
    }
    
    public static Arcade2048Options.ConnectionStringOptions GetConnectionStringOptions(this IConfiguration configuration)
    {
        return configuration
            .GetSection(Arcade2048Options.ConnectionStringOptions.SectionName)
            .Get<Arcade2048Options.ConnectionStringOptions>();
    }
    
    public static Arcade2048Options.AuthOptions GetAuthOptions(this IConfiguration configuration)
    {
        return configuration
            .GetSection(Arcade2048Options.AuthOptions.SectionName)
            .Get<Arcade2048Options.AuthOptions>();
    }

    public static string GetJaegerHostValue(this IConfiguration configuration)
    {
        return configuration
            .GetSection(Arcade2048Options.SectionName)
            .GetSection(nameof(Arcade2048Options.JaegerHost)).Value;
    }
}