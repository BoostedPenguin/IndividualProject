using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;


namespace netCore_testing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly FirestoreDb db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, FirestoreDb db)
        {
            this.db = db;
            _logger = logger;
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
        public WeatherForecast GetItem(int id)
        {
            return new WeatherForecast() { Date = DateTime.MaxValue, Summary = "Successfully added a new item :))", TemperatureC = id };
        }

        public async void AddData()
        {
            DocumentReference docRef = db.Collection("users").Document();
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "first_name", "Added through DI" },
                { "age", 5 },
            };
            await docRef.SetAsync(user);

        }

        //public static async void GetData()
        //{
        //    var docRef = await db.Collection("users").GetSnapshotAsync();

        //    foreach (var a in docRef)
        //    {
        //        Dictionary<string, object> document = a.ToDictionary();
        //    }
        //}
    }
}
