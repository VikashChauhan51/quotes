using Microsoft.AspNetCore.RateLimiting;
using Quotes.API.Configurations;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Quotes.API.Policies;

public class UserBasedRateLimitingPolicy : IRateLimiterPolicy<string>
{
    private readonly TokenBucketRateLimiterConfig _tokenBucketRateLimiterConfig;
    public UserBasedRateLimitingPolicy(TokenBucketRateLimiterConfig tokenBucketRateLimiterConfig)
    {
        _tokenBucketRateLimiterConfig = tokenBucketRateLimiterConfig ?? throw new ArgumentException(nameof(tokenBucketRateLimiterConfig));
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
        return RateLimitPartition.GetTokenBucketLimiter(ownerId, _ =>
        new TokenBucketRateLimiterOptions
        {
            TokenLimit = _tokenBucketRateLimiterConfig.TokenLimit,
            QueueProcessingOrder = (QueueProcessingOrder)_tokenBucketRateLimiterConfig.QueueProcessingOrder,
            QueueLimit = _tokenBucketRateLimiterConfig.QueueLimit,
            ReplenishmentPeriod = TimeSpan.FromSeconds(_tokenBucketRateLimiterConfig.ReplenishmentPeriodSeconds),
            TokensPerPeriod = _tokenBucketRateLimiterConfig.TokensPerPeriod,
            AutoReplenishment = _tokenBucketRateLimiterConfig.AutoReplenishment
        });
    }
}
