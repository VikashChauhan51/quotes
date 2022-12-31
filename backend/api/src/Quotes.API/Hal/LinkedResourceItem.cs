using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Quotes.API.Hal;

public class LinkedResourceItem<T>: LinkedResource
{
    [JsonProperty(PropertyName = "_data")]
    public T Data { get; init; } = default!;
}
