using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Quotes.API.Entities;

public class Quote
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }
    public string OwnerId { get; set; } = string.Empty;
}

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.Message).IsRequired().HasMaxLength(150);
        builder.Property(a => a.CreatedOn).IsRequired();
        builder.Property(a => a.OwnerId).IsRequired().HasMaxLength(50);
        builder.HasKey(a => a.Id);
        builder.ToTable("Quotes");
    }
}