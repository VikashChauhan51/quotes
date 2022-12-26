using Microsoft.EntityFrameworkCore;
using Quotes.API.Entities;

namespace Quotes.API.DbContexts;

public class QuotesContext : DbContext
{
    public QuotesContext(DbContextOptions<QuotesContext> options)
       : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    // base DbContext constructor ensures that Books and Authors are not null after
    // having been constructed.  Compiler warning ("uninitialized non-nullable property")
    // can safely be ignored with the "null-forgiving operator" (= null!)

    public DbSet<Quote> Quotes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuoteConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
