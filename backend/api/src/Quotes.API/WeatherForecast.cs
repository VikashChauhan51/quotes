using Quotes.API.Hal;

namespace Quotes.API;

public class WeatherForecast : LinkedResource
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

public class WeatherForecasts : LinkedResourceCollection<WeatherForecast>
{

}