using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Quotes.API.Extensions;
using Quotes.API.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;
using Quotes.API.Constants;

namespace Quotes.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/quotes")]
public class QuotesController : ControllerBase
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IValidator<QuoteModel> _quoteValidator;
    public QuotesController(IQuoteRepository quoteRepository, IMapper mapper, ILogger logger, IValidator<QuoteModel> quoteValidator)
    {
        _quoteRepository = quoteRepository ?? throw new ArgumentNullException(nameof(quoteRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _quoteValidator = quoteValidator ?? throw new ArgumentNullException(nameof(quoteValidator));
    }





    [HttpGet("{quoteId}", Name = "GetQuote")]
    [Authorize(Policy = ApiConstants.MustOwnQuoteAuthorizationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(Guid quoteId)
    {
        var quoteFromRepo = await _quoteRepository.GetQuoteAsync(quoteId);
        if (quoteFromRepo is null)
        {
            return NotFound();
        }
        var quoteToReturn = _mapper.Map<QuoteModel>(quoteFromRepo);
        if (this.HttpContext.IsJsonHalAcceptType())
        {
            var quoteLinks = CreateLinksForQuote(quoteId);

            var response = new LinkedResourceItem<QuoteModel>
            {
                Data = quoteToReturn,
                Links = quoteLinks
            };
            return Ok(response);
        }
        else
        {
            return Ok(quoteToReturn);
        }
    }

    [HttpPost(Name = "AddQuote")]
    [Authorize(Policy = ApiConstants.CanAddQuoteAuthorizationPolicy)]
    [Authorize(Policy = ApiConstants.ClientCanWriteAuthorizationPolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] QuoteModel quote)
    {
         
        var validationResult = await _quoteValidator.ValidateAsync(quote);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);

            return BadRequest(ModelState);
        }
        var quoteEntity = _mapper.Map<Quote>(quote);

        var ownerId = this.HttpContext.GetOwnerId();
        if (ownerId is null)
        {
            _logger.Information("ownerId not found in HttpContext.");
            return Unauthorized();
        }
        quoteEntity.OwnerId = ownerId;


        // add and save.  
        _quoteRepository.AddQuote(quoteEntity);

        await _quoteRepository.SaveChangesAsync();

        var quoteToReturn = _mapper.Map<QuoteModel>(quoteEntity);

        if (this.HttpContext.IsJsonHalAcceptType())
        {
            var quoteLinks = CreateLinksForQuote(quoteEntity.Id);

            var response = new LinkedResourceItem<QuoteModel>
            {
                Data = quoteToReturn,
                Links = quoteLinks
            };
            return CreatedAtRoute("GetQuote",
            new { quoteId = quoteEntity.Id },
            response);
        }
        else
        {
            return CreatedAtRoute("GetQuote",
           new { quoteId = quoteEntity.Id },
           quoteToReturn);
        }


    }

    [HttpPut("{quoteId}", Name = "UpdateQuote")]
    [Authorize(ApiConstants.MustOwnQuoteAuthorizationPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put(Guid quoteId, [FromBody] QuoteModel quote)
    {
        var validationResult = await _quoteValidator.ValidateAsync(quote);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);

            return BadRequest(ModelState);
        }

        var quoteFromRepo = await _quoteRepository.GetQuoteAsync(quoteId);
        if (quoteFromRepo is null)
        {
            return NotFound();
        }

        _mapper.Map(quote, quoteFromRepo);

        _quoteRepository.UpdateQuote(quoteFromRepo);

        await _quoteRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{quoteId}", Name = "DeleteQuote")]
    [Authorize(ApiConstants.MustOwnQuoteAuthorizationPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid quoteId)
    {
        var quoteFromRepo = await _quoteRepository.GetQuoteAsync(quoteId);
        if (quoteFromRepo is null)
        {
            return NotFound();
        }
        _quoteRepository.DeleteQuote(quoteFromRepo);
        await _quoteRepository.SaveChangesAsync();

        return NoContent();

    }


    private IEnumerable<ILink> CreateLinksForQuote(Guid quoteId)
    {
        return new List<Link> {
         new Link
         {
             Href= Url.RouteUrl("GetQuote",  new { quoteId })!,
             Rel= "self",
             Method= HttpVerbs.Get
         },
        new Link
        {
            Href=Url.RouteUrl("AddQuote",new { })!,
            Rel="create_quote",
            Method=HttpVerbs.Post
        },
        new Link
        {
            Href= Url.RouteUrl("UpdateQuote", new { quoteId })!,
            Rel="update_quote",
            Method=HttpVerbs.Put
        },
        new Link
        {
            Href= Url.RouteUrl("DeleteQuote",  new { quoteId })!,
            Rel= "delete_quote",
            Method=HttpVerbs.Delete
        }

        };

    }

   

}
