using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quotes.Client.Services;
 

namespace Quotes.Client.Pages.Authentication;


[Authorize]
public class LogoutModel : PageModel
{
    private readonly IQuoteAuthenticationService _authenticationService;
    public LogoutModel(IQuoteAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }
    public async Task<IActionResult> OnGet()
    {
       
        var discoveryDocumentResponse= await _authenticationService.GetDiscoveryDocumentAsync();

        if (discoveryDocumentResponse.IsError)
        {
            throw new Exception(discoveryDocumentResponse.Error);
        }
        var accessTokenRevocationResponse = await _authenticationService
            .RevokeTokenAsync(new()
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = "quotesclient",
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
                ClientId = "quotesclient",
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

        return Page();
    }
}
