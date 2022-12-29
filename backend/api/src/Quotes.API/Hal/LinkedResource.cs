using Newtonsoft.Json;
 

namespace Quotes.API.Hal;
public abstract class LinkedResource
{
    [JsonProperty(PropertyName = "_links")]
    public IEnumerable<ILink> Links { get; init; } = new List<ILink>();

}
