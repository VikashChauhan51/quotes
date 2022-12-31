namespace Quotes.API.Configurations;

public class ConcurrencyLimiterConfig
{
    public int PermitLimit { get; set; }
    public int QueueProcessingOrder { get; set; }
    public int QueueLimit { get; set; }
}
