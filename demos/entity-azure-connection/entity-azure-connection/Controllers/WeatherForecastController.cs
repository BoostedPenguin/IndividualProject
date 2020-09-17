using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace entity_azure_connection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BloggingContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, BloggingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            Blog found;
            using (_context)
            {
                found = await _context.FindAsync<Blog>(id);
            }

            if(found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> Createitem(Blog item)
        {
            if (item == null) return NotFound();

            using(_context)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
            }

            return Ok(item);
        }
    }
}
