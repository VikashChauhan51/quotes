using Quotes.Client.Models;

namespace Quotes.Client.Services
{
    public interface IQuoteService
    {
        Task Delete(Link link);
        Task<QuoteResponse> Get(Link link);
        Task<QuoteResponse> Post(QuoteForCreation quote);
        Task Put(Link link, QuoteForCreation quote);
    }
}