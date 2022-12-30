namespace Quotes.API.Models
{
    public record QuoteModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;       
    }
}
