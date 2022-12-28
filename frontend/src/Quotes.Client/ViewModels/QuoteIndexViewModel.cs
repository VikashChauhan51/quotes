using Quotes.Client.Models;

namespace Quotes.Client.ViewModels;

public record QuoteIndexViewModel
{
    public IEnumerable<Quote> Quotes { get; private set; }
            = new List<Quote>();

    public QuoteIndexViewModel(IEnumerable<Quote> quotes)
    {
        Quotes = quotes;
    }
}
