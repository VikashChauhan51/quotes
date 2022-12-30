using Microsoft.AspNetCore.RateLimiting;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Quotes.API.Policies;

public class UserBasedRateLimitingPolicy : IRateLimiterPolicy<string>
{
    public UserBasedRateLimitingPolicy()
    {

    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
        (context, _) =>
        {

            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return new ValueTask();
        };

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var ownerId = httpContext.GetOwnerId() ?? httpContext.Request.Headers.Host.ToString();
        return RateLimitPartition.GetTokenBucketLimiter(ownerId, _ =>
        new TokenBucketRateLimiterOptions
        {
            TokenLimit = 5,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1),
            TokensPerPeriod = 5,
            AutoReplenishment = false
        });
    }
}
