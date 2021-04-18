using System;
using System.Collections.Generic;
using System.Linq;
using Hello5.Domain.Contract.Models.WeatherForecast;
using Hello5.Domain.Endpoint.Controllers.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Hello5.Domain.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : HelloContollerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecastResponseDto> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastResponseDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
