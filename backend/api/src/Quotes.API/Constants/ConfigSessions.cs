namespace Quotes.API.Constants;

public static class ConfigSessions
{
    public const string SqlServerConfig = "Sqlserver";
    public const string AuthenticationConfig = "Authentication";
    public const string ClientApplicationCanWriteScopes = "ClientApplicationCanWrite";
    public const string ClientApplicationCanReadScopes = "ClientApplicationCanRead";
    public const string ClientApplicationFullAccessScopes = "ClientApplicationFullAccess";
    public const string TokenBucketRateLimiterConfig = "RateLimiting:TokenBucketRateLimiter";
    public const string ConcurrencyRateLimiterConfig = "RateLimiting:ConcurrencyLimiter";
}
