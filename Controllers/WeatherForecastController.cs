using Microsoft.AspNetCore.Mvc;

namespace MyApiProject;

[ApiController]
[Route("weatherforecast")]
public class WeatherForecastController : ControllerBase
{

    private readonly IActionsinterface? _actionsService;

    public WeatherForecastController(IActionsinterface? actionsService)
    {
        _actionsService = actionsService;
    }

    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] Weathercontracts weatherContract)

   {

        if (_actionsService != null)
        {
            _actionsService.CreateWeatherForecast(weatherContract);
            
            return Ok (new { Message = "Weather forecast created successfully", Data = weatherContract } );
        }

        return BadRequest("Action service is not available.");
        
       
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        // // Here you would typically save the weatherContract to a database or perform other business logic.

        // return Ok(new { Message = "Weather forecast created successfully", Data = weatherContract });
    }
}
