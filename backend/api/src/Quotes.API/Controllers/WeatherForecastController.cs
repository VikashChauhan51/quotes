using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Quotes.API.Hal;
using Microsoft.AspNetCore.Http;

namespace Quotes.API.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Get()
        {

            Request.Headers.TryGetValue("Accept", out var accept);
             
            //if (!MediaTypeHeaderValue.TryParse(accept, out MediaTypeHeaderValue parseMediaType))
            //{
            //    return BadRequest();
            //}
            var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();

            if (accept.Contains("application/json"))
            {
                return Ok(items);
            }

            int count = 0;
            foreach (var item in items)
            {
                count++;
                var addCart = new Link
                {
                    Rel = "self",
                    Href = Url.RouteUrl("GetWeatherForecast", values: new { count }),
                    Method = HttpVerbs.Get
                };
                item.Links.Add(addCart);
            }

            return Ok(new WeatherForecasts { Items = items });

        }
    }
}