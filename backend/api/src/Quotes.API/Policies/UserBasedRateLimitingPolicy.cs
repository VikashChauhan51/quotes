using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Quotes.API.Configurations;
using RedisRateLimiting;
using StackExchange.Redis;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Quotes.API.Policies;

public class UserBasedRateLimitingPolicy : IRateLimiterPolicy<string>
{
    private readonly TokenBucketRateLimiterConfig _tokenBucketRateLimiterConfig;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public UserBasedRateLimitingPolicy(IConnectionMultiplexer connectionMultiplexer,
        IOptions<TokenBucketRateLimiterConfig> tokenBucketRateLimiterConfig)
    {
        _tokenBucketRateLimiterConfig = tokenBucketRateLimiterConfig?.Value ?? throw new ArgumentException(nameof(tokenBucketRateLimiterConfig));
        _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentException(nameof(connectionMultiplexer));
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
        (context, _) =>
        {

            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            //TODO: log into DB if needed
            return new ValueTask();
        };

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var ownerId = httpContext.GetOwnerId() ?? httpContext.Request.Headers.Host.ToString();
        return RedisRateLimitPartition.GetTokenBucketRateLimiter(ownerId, _ =>
        new RedisTokenBucketRateLimiterOptions
        {
            TokenLimit = _tokenBucketRateLimiterConfig.TokenLimit,
            ConnectionMultiplexerFactory = () => _connectionMultiplexer,
            ReplenishmentPeriod = TimeSpan.FromSeconds(_tokenBucketRateLimiterConfig.ReplenishmentPeriodSeconds),
            TokensPerPeriod = _tokenBucketRateLimiterConfig.TokensPerPeriod
        });
    }
}
