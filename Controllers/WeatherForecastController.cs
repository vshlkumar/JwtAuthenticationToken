using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicCoreApplication.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BasicCoreApplication.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly DatabaseContext _context;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, DatabaseContext context)
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

		[HttpGet]
		[Route("GetListOfUsers")]
		public async Task<JsonResult> GetListOfUsers()
		{
			return new JsonResult(await _context.Employees.ToListAsync());
		}
	}
}
