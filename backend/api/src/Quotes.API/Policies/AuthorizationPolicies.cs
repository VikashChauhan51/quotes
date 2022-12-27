using Microsoft.AspNetCore.Authorization;

namespace Quotes.API.Policies;

public static class AuthorizationPolicies
{

    public static AuthorizationPolicy CanAddQuote()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("AddQuote")
            .Build();
    }
}
