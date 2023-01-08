using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Quotes.Client.Controllers;

public class AuthenticationController : Controller
{
    private readonly IQuoteAuthenticationService _authenticationService;
    public AuthenticationController(IQuoteAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [Authorize]
    public async Task Logout()
    {
        var discoveryDocumentResponse = await _authenticationService.GetDiscoveryDocumentAsync();

        if (discoveryDocumentResponse.IsError)
        {
            throw new Exception(discoveryDocumentResponse.Error);
        }
        var accessTokenRevocationResponse = await _authenticationService
            .RevokeTokenAsync(new()
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = "quoteclient",
                ClientSecret = "secret",
                Token = await this.HttpContext.GetTokenAsync(
                    OpenIdConnectParameterNames.AccessToken)
            });

        if (accessTokenRevocationResponse.IsError)
        {
            throw new Exception(accessTokenRevocationResponse.Error);
        }

        var refreshTokenRevocationResponse = await _authenticationService
            .RevokeTokenAsync(new()
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = "quoteclient",
                ClientSecret = "secret",
                Token = await this.HttpContext.GetTokenAsync(
                OpenIdConnectParameterNames.RefreshToken)
            });

        if (refreshTokenRevocationResponse.IsError)
        {
            throw new Exception(accessTokenRevocationResponse.Error);
        }

        // Clears the  local cookie
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirects to the IDP linked to scheme
        // "OpenIdConnectDefaults.AuthenticationScheme" (oidc)
        // so it can clear its own session/cookie
        await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
}
