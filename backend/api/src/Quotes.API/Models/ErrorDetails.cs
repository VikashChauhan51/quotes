using System.Text.Json;

namespace Quotes.API.Models;

public record ErrorDetails
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
