using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quotes.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/quotes-search")]
public class QuotesSearchController : ControllerBase
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IMapper _mapper;
    public QuotesSearchController(IQuoteRepository quoteRepository, IMapper mapper)
    {
        _quoteRepository = quoteRepository ?? throw new ArgumentNullException(nameof(quoteRepository));
        _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet(Name = "Search")]
    [Authorize("MustOwnQuote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<QuoteModel>>> Get()
    {
        var ownerId = HttpContext.GetOwnerId();
        if (ownerId is null)
        {

            return Unauthorized();
        }

        // get from repo
        var quotesFromRepo = await _quoteRepository.GetQuotesAsync(ownerId);

        // map to model
        var quotesToReturn = _mapper.Map<IEnumerable<QuoteModel>>(quotesFromRepo);

        // return
        return Ok(quotesToReturn);
    }
}
