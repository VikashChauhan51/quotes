using Dapr;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Grpc.Net.Client.Configuration;
using Polly;
using Polly.Retry;

namespace Quotes.API.Extensions;

public static class DaprConfigurationExtensions
{
    public static IConfigurationBuilder UseDaprAppConfiguration(this IConfigurationBuilder configurationBuilder, string storeName, IReadOnlyCollection<DaprSecretDescriptor> secretDescriptor, int maxRetryCount = 5, string? grpcEndpoint = null, string? httpEndpoint = null)
    {
        if (string.IsNullOrEmpty(storeName))
            throw new ArgumentNullException(nameof(storeName));

        if (!secretDescriptor?.Any() ?? false)
            throw new ArgumentNullException(nameof(secretDescriptor));

        var daprClientBuilder = new DaprClientBuilder();

        if (!string.IsNullOrEmpty(grpcEndpoint))
        {
            daprClientBuilder.UseGrpcEndpoint(grpcEndpoint);
        }

        if (!string.IsNullOrEmpty(httpEndpoint))
        {
            daprClientBuilder.UseHttpEndpoint(httpEndpoint);
        }
        var daprClient = daprClientBuilder.Build();
        return Policy.Handle<DaprException>().WaitAndRetry(maxRetryCount, maxRetryCount => TimeSpan.FromSeconds(Math.Pow(2.0, maxRetryCount)))
            .Execute(() => configurationBuilder.AddDaprSecretStore(storeName, secretDescriptor!, daprClient));
    }
}
