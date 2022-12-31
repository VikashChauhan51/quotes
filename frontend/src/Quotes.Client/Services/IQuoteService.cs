using Quotes.Client.Models;

namespace Quotes.Client.Services
{
    public interface IQuoteService
    {
        Task Delete(ILink link);
        Task<QuoteResponse> Get(ILink link);
        Task<QuoteResponse> Post(QuoteForCreation quote);
        Task Put(ILink link, QuoteForCreation quote);
    }
}