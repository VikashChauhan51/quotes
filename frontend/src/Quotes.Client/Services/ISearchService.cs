using Quotes.Client.Models;

namespace Quotes.Client.Services
{
    public interface ISearchService
    {
        Task<QuotesCollectionResponse> FindAsync(SearchParameters parameters);
    }
}