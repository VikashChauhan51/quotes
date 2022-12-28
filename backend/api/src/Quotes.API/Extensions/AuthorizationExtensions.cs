using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Quotes.API.Extensions;

public static class AuthorizationExtensions
{

    public static string? GetOwnerId(this HttpContext context)
    {
        return context.User.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value;
    }
    public static string? GetUseName(this HttpContext context)
    {
        return context.User.Claims
                .FirstOrDefault(c => c.Type == "name")?.Value;
    }
    public async static Task<string?> GetIdTokenAsync(this HttpContext context)
    {
        return await context
            .GetTokenAsync(OpenIdConnectParameterNames.IdToken);
    }
    public async static Task<string?> GetAccessTokenAsync(this HttpContext context)
    {
        return await context
            .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
    }
    public async static Task<string?> GetRefreshTokenAsync(this HttpContext context)
    {
        return await context
            .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
    }

    public static string? GetOwnerId(this AuthorizationHandlerContext context)
    {
        return context.User.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value;
    }
}
