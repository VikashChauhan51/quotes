
using System.Text.Json.Serialization;

namespace Quotes.Client.Hal;

public abstract class LinkedResource
{
    [JsonPropertyName("_links")]
    public List<Link> Links { get; init; } = new List<Link>();

}