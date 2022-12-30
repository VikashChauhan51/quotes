using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Quotes.Client.Models;
using Quotes.Client.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace Quotes.Client.Controllers;

[Authorize]
public class QuoteController : Controller
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<QuoteController> _logger;
    private readonly string rootEndpoint = "/api/v1/root";

    public QuoteController(IHttpClientFactory httpClientFactory,
        ILogger<QuoteController> logger)
    {
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ActionResult> Index()
    {
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get, rootEndpoint);

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {


            var links = await JsonConverterHelper.DeserializeAsync<Link[]>(responseStream);
            TempData["Links"] = JsonConverterHelper.Serialize(links);
        }

        //using (var responsestream = await response.content.readasstreamasync())
        //{
        //    var quotes = await jsonserializer.deserializeasync<list<quote>>(responsestream);
        //    return view(new quoteindexviewmodel(quotes ?? new list<quote>()));
        //}
        return View(new QuoteIndexViewModel(new List<Quote>()));
    }

    public async Task<ActionResult> DetailsQuote(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get, rootEndpoint);

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {


            var links = await JsonConverterHelper.DeserializeAsync<Link[]>(responseStream);
            TempData["Links"] = JsonConverterHelper.Serialize(links);
        }

        //using (var responsestream = await response.content.readasstreamasync())
        //{
        //    var quotes = await jsonserializer.deserializeasync<list<quote>>(responsestream);
        //    return view(new quoteindexviewmodel(quotes ?? new list<quote>()));
        //}
        return View(new QuoteIndexViewModel(new List<Quote>()));
    }


    public async Task<IActionResult> EditQuote(Guid id)
    {

        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/v1/quotes/{id}");

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            var deserializedQuote = await JsonSerializer.DeserializeAsync<Quote>(responseStream);

            if (deserializedQuote == null)
            {
                throw new Exception("Deserialized quote must not be null.");
            }

            var editQuoteViewModel = new QuoteForUpdation()
            {
                Id = deserializedQuote.Id,
                Message = deserializedQuote.Message
            };

            return View(editQuoteViewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuote(QuoteForUpdation editQuoteViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var quoteForUpdate = new QuoteForCreation { Message = editQuoteViewModel.Message };
        var serializedQuoteForUpdate = JsonSerializer.Serialize(quoteForUpdate);

        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/v1/quotes/{editQuoteViewModel.Id}")
        {
            Content = new StringContent(
                serializedQuoteForUpdate, new MediaTypeHeaderValue(
                "application/json"))
        };

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteQuote(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"/api/v1/quotes/{id}");

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    [Authorize(Policy = "UserCanAddQuote")]
    public async Task<IActionResult> AddQuote()
    {
        
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get, rootEndpoint);

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {


            var links = await JsonConverterHelper.DeserializeAsync<Link[]>(responseStream);
            TempData["quoteLinks"] = JsonConverterHelper.Serialize(links);
            return View(new QuoteForCreation { Links = links! });
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "UserCanAddQuote")]
    public async Task<IActionResult> AddQuote(QuoteForCreation addQuoteViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var links = JsonConverterHelper.Deserialize<Link[]>(TempData["quoteLinks"].ToString());    
        var endPoint = links?.First(x => x.Method == HttpVerbs.Post).Href;

        var serializedQuoteForCreation = JsonSerializer.Serialize(new QuoteDto { Message = addQuoteViewModel.Message });

        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Post, endPoint)
        {
            Content = new StringContent(serializedQuoteForCreation, new MediaTypeHeaderValue(HeaderKeys.Json))
        };

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();
        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {


            var quotewithLinks = await JsonConverterHelper.DeserializeAsync<QuoteForCreation>(responseStream);
            TempData["quoteLinks"] = JsonConverterHelper.Serialize(quotewithLinks!.Links);
        }

        return RedirectToAction("Index");
    }
}
