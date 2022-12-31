using Microsoft.AspNetCore.Authorization;

namespace Quotes.API.Policies;

public static class AuthorizationPolicies
{

    public const string AddQuoteRole = "AddQuote";
    public static AuthorizationPolicy CanAddQuote()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(AddQuoteRole)
            .Build();
    }
}
