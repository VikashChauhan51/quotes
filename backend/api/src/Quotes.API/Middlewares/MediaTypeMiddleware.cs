using Quotes.API.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Quotes.API.Middlewares;

public class MediaTypeMiddleware : IMiddleware
{
    private readonly string[] methods = new string[] { HttpMethods.Post, HttpMethods.Put, HttpMethods.Patch };
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestMethod = context.Request.Method.ToUpper();

        if (methods.Contains(requestMethod) &&
            !string.Equals(MediaTypeHeaderValue.Parse(context.Request.ContentType).MediaType.Value, HeaderKeys.Json, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
        }
        else
        {
            await next(context);
        }
    }
}
