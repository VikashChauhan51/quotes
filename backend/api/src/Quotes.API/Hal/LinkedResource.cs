using Newtonsoft.Json;
 

namespace Quotes.API.Hal;
public abstract class LinkedResource
{
    [JsonProperty(PropertyName = "_links")]
    public IList<ILink> Links { get; init; } = new List<ILink>();

}
