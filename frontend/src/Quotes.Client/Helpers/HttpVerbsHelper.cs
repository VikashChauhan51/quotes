namespace Quotes.Client.Helpers;

public static class HttpVerbsHelper
{


    public static HttpMethod GetMethod(HttpVerbs verb) => verb switch
    {
        HttpVerbs.Get => HttpMethod.Get,
        HttpVerbs.Head => HttpMethod.Head,
        HttpVerbs.Post => HttpMethod.Post,
        HttpVerbs.Put => HttpMethod.Put,
        HttpVerbs.Patch => HttpMethod.Patch,
        HttpVerbs.Delete => HttpMethod.Delete,
        HttpVerbs.Options => HttpMethod.Options

    };
}
