using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quotes.Client.Models;

namespace Quotes.Client.Controllers;

[Authorize]
public class QuoteController : Controller
{

    private readonly IQuoteService _quoteService;
    public QuoteController(IQuoteService quoteService)
    {
        _quoteService = quoteService ?? throw new ArgumentNullException(nameof(quoteService));
    }
    // [Authorize(Policy = "UserCanAddQuote")]
    public IActionResult AddQuote()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddQuote(QuoteForCreation quote)
    {
        if (ModelState.IsValid)
        {
             await _quoteService.Post(quote);
            return RedirectToAction("Index","Home");
        }
        return View();
    }
}
