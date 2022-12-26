namespace Quotes.API.Repositories
{
    public interface IQuoteRepository : IDisposable, IAsyncDisposable
    {
        void AddQuote(Quote quote);
        void DeleteQuote(Quote quote);
        Task<Quote?> GetQuoteAsync(Guid id);
        Task<IEnumerable<Quote>> GetQuotesAsync(string ownerId);
        Task<bool> IsQuoteOwnerAsync(Guid id, string ownerId);
        Task<bool> QuoteExistsAsync(Guid id);
        Task<bool> SaveChangesAsync();
        void UpdateQuote(Quote quote);
    }
}