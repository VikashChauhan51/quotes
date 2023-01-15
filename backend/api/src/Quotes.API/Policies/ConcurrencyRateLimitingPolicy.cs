using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Quotes.API.Configurations;
using RedisRateLimiting;
using StackExchange.Redis;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Quotes.API.Policies
{
    public class ConcurrencyRateLimitingPolicy : IRateLimiterPolicy<string>
    {
        private readonly ConcurrencyLimiterConfig _concurrencyLimiterConfig;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public ConcurrencyRateLimitingPolicy(IConnectionMultiplexer connectionMultiplexer,
            IOptions<ConcurrencyLimiterConfig> concurrencyLimiterConfig)
        {
            _concurrencyLimiterConfig = concurrencyLimiterConfig?.Value ?? throw new ArgumentNullException(nameof(concurrencyLimiterConfig));
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
            var host = httpContext.Request.Headers.Host.ToString();

            // one request at given moment of time per host
            return RedisRateLimitPartition.GetConcurrencyRateLimiter(host, _ => new RedisConcurrencyRateLimiterOptions
            {
                PermitLimit = _concurrencyLimiterConfig.PermitLimit,
                QueueLimit = _concurrencyLimiterConfig.QueueLimit,
                ConnectionMultiplexerFactory = () => _connectionMultiplexer
            });
        }
    }
}
