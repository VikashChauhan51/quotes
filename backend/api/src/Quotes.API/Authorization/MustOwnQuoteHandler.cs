using Microsoft.AspNetCore.Authorization;

namespace Quotes.API.Authorization;

public class MustOwnQuoteHandler : AuthorizationHandler<MustOwnQuoteRequirement>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MustOwnQuoteHandler(IQuoteRepository quoteRepository, IHttpContextAccessor httpContextAccessor)
    {
        _quoteRepository = quoteRepository ??
            throw new ArgumentNullException(nameof(quoteRepository));
        _httpContextAccessor = httpContextAccessor ??
            throw new ArgumentNullException(nameof(httpContextAccessor));
    }



    protected override async Task HandleRequirementAsync( AuthorizationHandlerContext context, MustOwnQuoteRequirement requirement)
    {
        var quoteId = _httpContextAccessor.HttpContext?
            .GetRouteValue("id")?.ToString();

        if (!Guid.TryParse(quoteId, out Guid quoteIdAsGuid))
        {
            context.Fail();
            return;
        }

        // get the sub claim
        var ownerId = context.GetOwnerId();
        // if it cannot be found, the handler fails 
        if (ownerId is null)
        {
            context.Fail();
            return;
        }

        if (!await _quoteRepository.IsQuoteOwnerAsync(quoteIdAsGuid, ownerId))
        {
            context.Fail();
            return;
        }

        // all checks out
        context.Succeed(requirement);
    }
}
