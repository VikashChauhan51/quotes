using Microsoft.AspNetCore.Http.HttpResults;
using Quotes.API.Constants;

namespace Quotes.API.Extensions;

public static class HeadersExtensions
{
    public static bool IsJsonAcceptType(this HttpContext context)
    {
        var accept = context.Request.Headers.Accept.ToString().Split(',');
        return accept.Any(x => x is not null && x.Equals(HeaderKeys.Json, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsJsonHalAcceptType(this HttpContext context)
    {
        var accept = context.Request.Headers.Accept.ToString().Split(',');
        return accept.Any(x => x is not null && x.Equals(HeaderKeys.HalJson, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsJsonContentType(this HttpContext context)
    {
        var contentType = context.Request.Headers.ContentType.ToString().Split(',');
        return contentType.Any(x => x is not null && x.Equals(HeaderKeys.Json, StringComparison.OrdinalIgnoreCase));
    }
    public static bool IsJsonHalContentType(this HttpContext context)
    {
        var contentType = context.Request.Headers.ContentType.ToString().Split(',');
        return contentType.Any(x => x is not null && x.Equals(HeaderKeys.HalJson, StringComparison.OrdinalIgnoreCase));
    }
}
