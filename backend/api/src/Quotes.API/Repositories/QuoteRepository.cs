using Microsoft.EntityFrameworkCore;
using Quotes.API.DbContexts;
using Quotes.API.Models;

namespace Quotes.API.Repositories;

public class QuoteRepository : IQuoteRepository
{
    private readonly QuotesContext _context;

    public QuoteRepository(QuotesContext quotesContext)
    {
        _context = quotesContext ?? throw new ArgumentNullException(nameof(quotesContext));
    }

    public async Task<bool> QuoteExistsAsync(Guid id)
    {
        return await _context.Quotes.AnyAsync(i => i.Id == id);
    }

    public async Task<Quote?> GetQuoteAsync(Guid id)
    {
        return await _context.Quotes.FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<PagedModel<Quote>> GetQuotesAsync(string ownerId, int limit, int page, string text, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(text))
        {
            return await _context.Quotes
           .AsNoTracking()
           .Where(i => i.OwnerId == ownerId)
           .OrderBy(i => i.CreatedOn).PaginateAsync(page, limit, cancellationToken);
        }

        return await _context.Quotes
           .AsNoTracking()
           .Where(i => i.OwnerId == ownerId && i.Message.Contains(text))
           .OrderBy(i => i.CreatedOn).PaginateAsync(page, limit, cancellationToken);
    }

    public async Task<bool> IsQuoteOwnerAsync(Guid id, string ownerId)
    {
        return await _context.Quotes
            .AnyAsync(i => i.Id == id && i.OwnerId == ownerId);
    }

    public void AddQuote(Quote quote)
    {
        quote.CreatedOn = DateTimeOffset.UtcNow;
        _context.Quotes.Add(quote);
    }

    public void UpdateQuote(Quote quote)
    {
        quote.CreatedOn = DateTimeOffset.UtcNow;
        _context.Quotes.Update(quote);
    }

    public void DeleteQuote(Quote quote)
    {
        _context.Quotes.Remove(quote);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }


    ~QuoteRepository() => Dispose(false);
    public void Dispose()
    {
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            //dispose managed state (managed objects)
            if (_context is not null)
            {
                _context.Dispose();
            }
        }

    }
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_context is not null)
        {
            await _context.DisposeAsync().ConfigureAwait(false);
        }
    }
}
