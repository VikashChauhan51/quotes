using Microsoft.AspNetCore.Routing;
using Quotes.Client.Hal;
using Quotes.Client.Models;

namespace Quotes.Client.Services;

public class SearchService : ISearchService
{
    private readonly HttpClient _httpClient;
    private readonly IRootService _rootService;
    public SearchService(HttpClient httpClient, IRootService rootService)
    {
        _rootService = rootService ?? throw new ArgumentNullException(nameof(rootService));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<QuotesCollectionResponse> FindAsync(SearchParameters parameters)
    {
        var links = await _rootService.GetLinksAsync();

        if (links is null)
        {
            return new QuotesCollectionResponse();
        }
        var link = links.First(x => x.Rel == "get_quotes");

        var request = new HttpRequestMessage(
        HttpVerbsHelper.GetMethod(link.Method),
        $"{link.Href}?limit={parameters.limit}&page={parameters.page}&searchQuery={parameters.searchQuery}");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            return await JsonConverterHelper.DeserializeAsync<QuotesCollectionResponse>(responseStream) ?? new QuotesCollectionResponse();

        }
    }
}
