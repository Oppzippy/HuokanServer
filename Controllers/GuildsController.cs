using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers
{
	[ApiController]
	[Route("/guilds2")]
	public class GuildsController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			return new[] {
				new WeatherForecast{
					Date = DateTime.UtcNow,
					TemperatureC = 5,
					Summary = "Test2!",
				}
			};
		}
	}
}
