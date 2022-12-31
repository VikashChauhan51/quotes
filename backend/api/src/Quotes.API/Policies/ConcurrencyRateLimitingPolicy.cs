using Microsoft.AspNetCore.RateLimiting;
using Quotes.API.Configurations;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Quotes.API.Policies
{
    public class ConcurrencyRateLimitingPolicy : IRateLimiterPolicy<string>
    {
        private readonly ConcurrencyLimiterConfig _concurrencyLimiterConfig;
        public ConcurrencyRateLimitingPolicy(ConcurrencyLimiterConfig concurrencyLimiterConfig)
        {
            _concurrencyLimiterConfig = concurrencyLimiterConfig ?? throw new ArgumentNullException(nameof(concurrencyLimiterConfig));
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
            var host = httpContext.Request.Headers.Host.ToString();

            // one request at given moment of time per host
            return RateLimitPartition.GetConcurrencyLimiter(host, _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = _concurrencyLimiterConfig.PermitLimit,
                QueueProcessingOrder = (QueueProcessingOrder)_concurrencyLimiterConfig.QueueProcessingOrder,
                QueueLimit = _concurrencyLimiterConfig.QueueLimit
            });
        }
    }
}
