using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quotes.Client.Services;

namespace Quotes.Client.Pages
{
    public class AddQuoteModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IQuoteService _quoteService;
        public AddQuoteModel(ILogger<IndexModel> logger, IQuoteService quoteService)
        {
            _logger = logger;
            _quoteService = quoteService;
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            return Page();
        }
    }
}
