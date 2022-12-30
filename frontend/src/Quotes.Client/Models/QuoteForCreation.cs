using System.ComponentModel.DataAnnotations;

namespace Quotes.Client.Models;

public class QuoteForCreation: LinkedResource
{

    [Required]
    [MaxLength(150)]
    public string Message { get; set; } = string.Empty;
}
