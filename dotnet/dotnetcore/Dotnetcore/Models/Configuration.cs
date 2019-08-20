namespace Dotnetcore.Models
{
    public class Configuration
    {
        public LoggingConfig Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string AuthTest { get; set; }
    }
}
