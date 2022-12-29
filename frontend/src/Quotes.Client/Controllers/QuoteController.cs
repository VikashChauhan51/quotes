using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;
using System.Text.Json;
using Quotes.Client.Models;
using Quotes.Client.ViewModels;
using System.Net.Http.Headers;

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
        await LogIdentityInformation(); 
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get, rootEndpoint);

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            var quotes = await JsonSerializer.DeserializeAsync<List<Quote>>(responseStream);
            return View(new QuoteIndexViewModel(quotes ?? new List<Quote>()));
        }
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

        var quoteForUpdate = new QuoteForCreation {Message= editQuoteViewModel.Message };
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
    public IActionResult AddQuote()
    {
        return View();
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

        var serializedQuoteForCreation = JsonSerializer.Serialize(addQuoteViewModel);

        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/quotes")
        {
            Content = new StringContent(serializedQuoteForCreation, new MediaTypeHeaderValue("application/json"))
        };

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    public async Task LogIdentityInformation()
    {
        // get the saved identity token
        var identityToken = await HttpContext
            .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

        // get the saved access token
        var accessToken = await HttpContext
            .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        // get the refresh token
        var refreshToken = await HttpContext
            .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        var userClaimsStringBuilder = new StringBuilder();
        foreach (var claim in User.Claims)
        {
            userClaimsStringBuilder.AppendLine(
                $"Claim type: {claim.Type} - Claim value: {claim.Value}");
        }

        // log token & claims
        _logger.LogInformation($"Identity token & user claims: " +
            $"\n{identityToken} \n{userClaimsStringBuilder}");
        _logger.LogInformation($"Access token: " +
            $"\n{accessToken}");
        _logger.LogInformation($"Refresh token: " +
            $"\n{refreshToken}");
    }
}
