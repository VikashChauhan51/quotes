using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quotes.API.Constants;
using System.Text.Json;

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
    [Authorize(ApiConstants.MustOwnQuoteAuthorizationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<QuoteModel>>> Get([FromQuery] UrlQueryParameters urlQueryParameters,
            CancellationToken cancellationToken)
    {
        var ownerId = HttpContext.GetOwnerId();
        if (ownerId is null)
        {

            return Unauthorized();
        }

        // get from repo
        var quotesFromRepo = await _quoteRepository.GetQuotesAsync(ownerId, urlQueryParameters.limit, urlQueryParameters.page, urlQueryParameters.searchQuery, cancellationToken);
        // map to model
        var quotesToReturn = _mapper.Map<IEnumerable<QuoteModel>>(quotesFromRepo.Items);
        var paginationMetadata = new
        {
            totalCount = quotesFromRepo.TotalItems,
            pageSize = quotesFromRepo.PageSize,
            currentPage = quotesFromRepo.CurrentPage,
            totalPages = quotesFromRepo.TotalPages
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        if (this.HttpContext.IsJsonHalAcceptType())
        {
            // create links
            var links = CreateLinksForQuotes(urlQueryParameters,
                quotesFromRepo.HasNext,
                quotesFromRepo.HasPrevious);

            var quotesWithLinks = CreateLinksForQuotes(quotesToReturn);
            var response = new QuotesPageCollectionModel
            {
                Items = quotesWithLinks,
                Links = links
            };
            return Ok(response);
        }
         
        return Ok(quotesToReturn);

    }

    private IEnumerable<LinkedResourceItem<QuoteModel>> CreateLinksForQuotes(IEnumerable<QuoteModel> quotes)
    {
        var list = new List<LinkedResourceItem<QuoteModel>>();
        foreach (var quote in quotes)
        {
            var quoteLinks = CreateLinksForQuote(quote.Id);
            list.Add(new LinkedResourceItem<QuoteModel>
            {
                Data = quote,
                Links = quoteLinks
            });

        }

        return list;

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
    private IEnumerable<ILink> CreateLinksForQuotes(
        UrlQueryParameters quoteResourceParameters,
        bool hasNext, bool hasPrevious)
    {
        var links = new List<Link>();

        links.Add(
            new(CreateQuoteResourceUri(quoteResourceParameters,
                LinkedResourceType.Current),
                "self",
                HttpVerbs.Get));

        if (hasNext)
        {
            links.Add(
                new(CreateQuoteResourceUri(quoteResourceParameters,
                    LinkedResourceType.NextPage),
                "nextPage",
                HttpVerbs.Get));
        }

        if (hasPrevious)
        {
            links.Add(
                new(CreateQuoteResourceUri(quoteResourceParameters,
                    LinkedResourceType.PreviousPage),
                "previousPage",
                HttpVerbs.Get));
        }

        return links;
    }

    private string? CreateQuoteResourceUri(
        UrlQueryParameters quoteResourceParameters,
        LinkedResourceType type)
    {
        switch (type)
        {
            case LinkedResourceType.PreviousPage:
                return Url.RouteUrl("Search",
                    new
                    {
                        limit = quoteResourceParameters.limit,
                        page = quoteResourceParameters.page-1,
                        searchQuery = quoteResourceParameters.searchQuery
                    });
            case LinkedResourceType.NextPage:
                return Url.RouteUrl("Search",
                    new
                    {
                        limit = quoteResourceParameters.limit,
                        page = quoteResourceParameters.page + 1,
                        searchQuery = quoteResourceParameters.searchQuery
                    });
            case LinkedResourceType.Current:
            default:
                return Url.RouteUrl("Search",
                    new
                    {
                        limit = quoteResourceParameters.limit,
                        page = quoteResourceParameters.page,
                        searchQuery = quoteResourceParameters.searchQuery
                    });
        }
    }
}
