namespace Quotes.API.Repositories
{
    public interface IQuoteRepository : IDisposable, IAsyncDisposable
    {
        void AddQuote(Quote quote);
        void DeleteQuote(Quote quote);
        Task<Quote?> GetQuoteAsync(Guid id);
        Task<PagedModel<Quote>> GetQuotesAsync(string ownerId, int limit, int page,string text, CancellationToken cancellationToken);
        Task<bool> IsQuoteOwnerAsync(Guid id, string ownerId);
        Task<bool> QuoteExistsAsync(Guid id);
        Task<bool> SaveChangesAsync();
        void UpdateQuote(Quote quote);
    }
}