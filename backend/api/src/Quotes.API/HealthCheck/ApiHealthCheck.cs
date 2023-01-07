using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace Quotes.API.HealthCheck;

public class ApiHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = HealthCheckResult.Healthy();
        Log.Information($"ApiHealthCheck: {result}");
        return Task.FromResult(result);
    }
}
