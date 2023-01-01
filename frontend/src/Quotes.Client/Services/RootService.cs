using Microsoft.AspNetCore.Routing;
using System.Net.Http;

namespace Quotes.Client.Services;

public class RootService : IRootService
{
    private readonly HttpClient _httpClient;
    private readonly string rootEndpoint = "/api/v1/root";
    public RootService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<Link>> GetLinksAsync()
    {
        var request = new HttpRequestMessage(
        HttpMethod.Get, rootEndpoint);

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            return await JsonConverterHelper.DeserializeAsync<Link[]>(responseStream) ?? Array.Empty<Link>();

        }
    }
}
