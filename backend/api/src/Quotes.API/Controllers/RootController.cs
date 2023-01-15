using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quotes.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/root")]
    //[Authorize]
    [ResponseCache(CacheProfileName = "Default5min")]
    public class RootController : ControllerBase
    {

        [HttpGet(Name = "GetRoot")]
        public ActionResult<IEnumerable<ILink>> Get()
        {
            // create links for root
            var links = new List<Link>
        {
            new Link(Url.RouteUrl("GetRoot", new { })!,
          "self",
          HttpVerbs.Get),

            new Link(Url.RouteUrl("AddQuote", new { })!,
        "create_quote",
        HttpVerbs.Post),

            new Link(Url.RouteUrl("Search", new { })!,
        "get_quotes",
        HttpVerbs.Get)
        };

            return Ok(links);
        }
    }
}
