using Dapr.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quotes.API.Configurations;

namespace Quotes.API.Services;

public class ContentStore<T> : IContentStore<T>
{
    private readonly DaprClient _daprClient;
    private readonly string _storeName;
    public ContentStore(DaprClient daprClient, IOptions<DaprConfig> daprConfig)
    {
        if (string.IsNullOrEmpty(daprConfig?.Value?.StatestoreName))
        {
            throw new ArgumentNullException(nameof(daprConfig));
        }

        _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        _storeName = daprConfig.Value.StatestoreName;
    }

    public async Task<T> FetchAsync(string contentId, CancellationToken token = default) => await _daprClient.GetStateAsync<T>(_storeName, contentId, cancellationToken: token);

    public async Task SaveAsync(string contentId, T value, CancellationToken token = default) => await _daprClient.SaveStateAsync(_storeName, contentId, value, cancellationToken: token);

    public async Task DeleteAsync(string contentId, CancellationToken token = default) => await _daprClient.DeleteStateAsync(_storeName, contentId, cancellationToken: token);
}
