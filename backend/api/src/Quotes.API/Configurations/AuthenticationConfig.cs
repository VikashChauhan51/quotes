namespace Quotes.API.Configurations;

public class AuthenticationConfig
{
    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string NameClaimType { get; set; } = string.Empty;
    public string RoleClaimType { get; set; } = string.Empty;
    public string[] Scopes { get; set; } = new string[0];
}
