using System.ComponentModel.DataAnnotations;

namespace Quotes.Client.Models;

public class QuoteForUpdation
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Message { get; set; } = string.Empty;
}
