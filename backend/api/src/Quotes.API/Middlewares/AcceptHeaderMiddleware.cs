using Quotes.API.Constants;
using System.Net;

namespace Quotes.API.Middlewares;

public class AcceptHeaderMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.IsJsonAcceptType() || context.IsJsonHalAcceptType())
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
        }

    }
}
