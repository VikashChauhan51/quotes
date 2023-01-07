using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quotes.Client.Models;
using Quotes.Client.Services;

namespace Quotes.Client.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly ISearchService _searchService;
    public IndexModel(ILogger<IndexModel> logger, ISearchService searchService) 
    {
        _logger = logger;
        _searchService= searchService;
    }

    public QuotesCollectionResponse QuotesData { get; set; }=new QuotesCollectionResponse();
    public async Task<IActionResult> OnGet()
    {
        //QuotesData = await _searchService.FindAsync(new SearchParameters());

        return Page();
    }
}