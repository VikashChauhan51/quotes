using IdentityModel.Client;
using System.Net.Http.Headers;
using Quotes.Client.Hal;
using Quotes.Client.Models;

namespace Quotes.Client.Services;

public class QuoteService : IQuoteService
{
    private readonly HttpClient _httpClient;
    private readonly IRootService _rootService;
    public QuoteService(HttpClient httpClient, IRootService rootService)
    {
        _rootService = rootService ?? throw new ArgumentNullException(nameof(rootService));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<QuoteResponse> Post(QuoteForCreation quote)
    {
        var links = await _rootService.GetLinksAsync();

        if (links is null)
        {
            return new QuoteResponse();
        }
        var link = links.First(x => x.Rel == "create_quote" && x.Method == HttpVerbs.Post);

        var serializedQuoteForCreation = JsonConverterHelper.Serialize(quote);
        var request = new HttpRequestMessage(
        HttpVerbsHelper.GetMethod(link.Method),
        $"{link.Href}");
        request.Content = new StringContent(serializedQuoteForCreation, new MediaTypeHeaderValue(HeaderKeys.Json));
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            return await JsonConverterHelper.DeserializeAsync<QuoteResponse>(responseStream) ?? new QuoteResponse();

        }
    }
    public async Task<QuoteResponse> Get(Link link)
    {


        var request = new HttpRequestMessage(
        HttpVerbsHelper.GetMethod(link.Method),
        $"{link.Href}");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            return await JsonConverterHelper.DeserializeAsync<QuoteResponse>(responseStream) ?? new QuoteResponse();

        }
    }

    public async Task Put(Link link, QuoteForCreation quote)
    {
        var serializedQuoteForCreation = JsonConverterHelper.Serialize(quote);

        var request = new HttpRequestMessage(
        HttpVerbsHelper.GetMethod(link.Method),
        $"{link.Href}");
        request.Content = new StringContent(serializedQuoteForCreation, new MediaTypeHeaderValue(HeaderKeys.Json));
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();


    }
    public async Task Delete(Link link)
    {
        var request = new HttpRequestMessage(
        HttpVerbsHelper.GetMethod(link.Method),
        $"{link.Href}");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

    }
}
