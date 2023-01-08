using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Quotes.IDP;

public static class Config
{
    public static string ClientBaseUrl { get; set; } = "https://quote-app.dev/"; //https://localhost:7131/
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles",
                "Your role(s)",
                new [] { "role" }),

        };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
        {
             new ApiResource("quoteapi",
                 "Quote API",
                 new [] { "role" })
             {
                 Scopes = { "quoteapi.fullaccess",
                     "quoteapi.read",
                     "quoteapi.write"},
                ApiSecrets = { new Secret("apisecret".Sha256()) }
             }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("quoteapi.fullaccess"),
                new ApiScope("quoteapi.read"),
                new ApiScope("quoteapi.write")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "quoteclient",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { $"{ClientBaseUrl}signin-oidc" },
                FrontChannelLogoutUri = $"{ClientBaseUrl}signout-oidc",
                PostLogoutRedirectUris = { $"{ClientBaseUrl}signout-callback-oidc" },
                AccessTokenType = AccessTokenType.Reference,
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowOfflineAccess = true,
                AllowedScopes =
                {
                         IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "quoteapi.fullaccess",
                        "quoteapi.read",
                        "quoteapi.write"
                },
                RequireConsent= true
            },
        };
}
