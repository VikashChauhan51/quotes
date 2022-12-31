namespace Quotes.API.Configurations;

public class TokenBucketRateLimiterConfig
{
    public int TokenLimit { get; set; }
    public int QueueProcessingOrder { get; set; }
    public int QueueLimit { get; set; }
    public int ReplenishmentPeriodSeconds { get; set; }
    public int TokensPerPeriod { get; set; }
    public bool AutoReplenishment { get; set; }
}
