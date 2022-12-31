using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Quotes.IDP;

public static class Config
{
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
                RedirectUris = { "https://localhost:7131/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7131/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7131/signout-callback-oidc" },
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
