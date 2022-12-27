using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quotes.API.Extensions;

public static class AuthorizationExtensions
{

    public static string? GetOwnerId(this HttpContext context)
    {
        return context.User.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value;
    }

    public static string? GetOwnerId(this AuthorizationHandlerContext context)
    {
        return context.User.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value;
    }
}
