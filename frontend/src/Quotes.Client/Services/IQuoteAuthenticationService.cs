using IdentityModel.Client;

namespace Quotes.Client.Services
{
    public interface IQuoteAuthenticationService
    {
        Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync();
        Task<TokenRevocationResponse> RevokeTokenAsync(TokenRevocationRequest request);
    }
}