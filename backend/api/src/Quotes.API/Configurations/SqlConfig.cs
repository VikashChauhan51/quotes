namespace Quotes.API.Configurations;

public record SqlConfig
{
    public string ServerName { get; set; } = string.Empty;
    public string DbName { get; set; } = string.Empty;
    public string Credentials { get; set; } = string.Empty;

}
