using Quotes.API.Constants;
using System.Net;

namespace Quotes.API.Middlewares;

public class HeaderMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.Headers.TryGetValue("Accept", out var accept);
        

        if (accept.Contains(HeaderKeys.Json) || accept.Contains(HeaderKeys.HalJson))
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
        }

    }
}
