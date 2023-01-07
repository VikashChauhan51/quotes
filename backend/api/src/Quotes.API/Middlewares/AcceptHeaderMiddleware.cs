using Quotes.API.Constants;
using Serilog;
using System.Linq;
using System.Net;

namespace Quotes.API.Middlewares;

public class AcceptHeaderMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        Log.Information($"request path: {context?.Request?.Path.Value}");
        if (IsHelthCheckRequest(context!) ||
            context!.IsJsonAcceptType() ||
            context!.IsJsonHalAcceptType())
        {
            await next(context!);
        }
        else
        {
            context!.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
        }

    }

    private bool IsHelthCheckRequest(HttpContext context) => context!.Request!.Path.Value.Equals("/health/startup") || context!.Request!.Path.Value.Equals("/healthz") || context!.Request!.Path.Value.Equals("/ready");
}
