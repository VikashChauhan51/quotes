namespace Quotes.Client.Models;

public class Quote: LinkedResource
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }
}
