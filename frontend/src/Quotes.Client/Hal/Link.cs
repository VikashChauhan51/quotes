

namespace Quotes.Client.Hal;

public class Link : ILink
{

    public string Href { get; init; } = default!;
    public string Rel { get; init; } = default!;
    public HttpVerbs Method { get; init; }

    public Link()
    {

    }
    public Link(string href, string rel, HttpVerbs method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}
