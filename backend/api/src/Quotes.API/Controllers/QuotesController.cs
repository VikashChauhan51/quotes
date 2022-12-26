using Microsoft.AspNetCore.Mvc;
using Quotes.API.DbContexts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quotes.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;
        public QuotesController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository ?? throw new ArgumentNullException(nameof(quoteRepository));
        }
        // GET: api/<QuotesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<QuotesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuotesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<QuotesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuotesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
