namespace Quotes.API.Constants;

public static class ApiConstants
{
    public const string UserBasedRateLimitingPolicy = "UserBasedRateLimiting";
    public const string DefaultRateLimitingPolicy = "DefaultRateLimit";
    public const string CanAddQuoteAuthorizationPolicy = "UserCanAddQuote";
    public const string ClientCanWriteAuthorizationPolicy = "ClientApplicationCanWrite";
    public const string MustOwnQuoteAuthorizationPolicy = "MustOwnQuote";
}
