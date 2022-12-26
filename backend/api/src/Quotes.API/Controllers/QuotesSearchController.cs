using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quotes.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/quotes-search")]
public class QuotesSearchController : ControllerBase
{
    [HttpGet(Name = "Search")]
    public IActionResult Get()
    {
        return Ok();
    }
    }
