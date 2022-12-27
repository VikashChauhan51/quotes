using Microsoft.AspNetCore.Authorization;

namespace Quotes.API.Authorization;

public class MustOwnQuoteRequirement: IAuthorizationRequirement
{
}
