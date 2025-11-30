using Microsoft.AspNetCore.Mvc;

namespace cityWatch_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet("hi")]
        public async Task<IActionResult> SayHi()
        {
            return Ok("Hello from citywatch api");
        }
    }
}
