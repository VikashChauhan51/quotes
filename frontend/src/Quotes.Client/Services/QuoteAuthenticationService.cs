using IdentityModel.Client;

namespace Quotes.Client.Services;

public class QuoteAuthenticationService : IQuoteAuthenticationService
{
    private readonly HttpClient _httpClient;
    public QuoteAuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
    {
        return await _httpClient.GetDiscoveryDocumentAsync();
    }

    public async Task<TokenRevocationResponse> RevokeTokenAsync(TokenRevocationRequest request)
    {
        return await _httpClient.RevokeTokenAsync(request);

    }
    
}
