using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quotes.Client.Models;
using System.Diagnostics;

namespace Quotes.Client.Controllers;


public class HomeController : Controller
{

    private readonly ISearchService _searchService;
    public HomeController(ISearchService searchService)
    {
        _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var quotesData = await _searchService.FindAsync(new SearchParameters());
        return View(quotesData);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}