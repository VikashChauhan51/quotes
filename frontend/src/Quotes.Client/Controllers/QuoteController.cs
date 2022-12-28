using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using Quotes.Client.Models;
using Quotes.Client.ViewModels;

namespace Quotes.Client.Controllers;

[Authorize]
public class QuoteController : Controller
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<QuoteController> _logger;

    public QuoteController(IHttpClientFactory httpClientFactory,
        ILogger<QuoteController> logger)
    {
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    // GET: QuoteController
    public async Task<ActionResult> Index()
    {
        await LogIdentityInformation();

        var id=Guid.NewGuid().ToString();   
        var httpClient = _httpClientFactory.CreateClient("APIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,$"/api/v1/quotes/{id}");

        var response = await httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            var quotes = await JsonSerializer.DeserializeAsync<List<Quote>>(responseStream);
            return View(new QuoteIndexViewModel(quotes ?? new List<Quote>()));
        }
    }

    // GET: QuoteController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: QuoteController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: QuoteController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: QuoteController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: QuoteController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: QuoteController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: QuoteController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
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
