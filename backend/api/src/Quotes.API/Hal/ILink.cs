namespace Quotes.API.Hal
{
    public interface ILink
    {
        string Href { get; init; }
        HttpVerbs Method { get; init; }
        string Rel { get; init; }
    }
}