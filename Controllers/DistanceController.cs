using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DistanceServer.Services;

namespace DistanceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : Controller
    {
        private readonly IDistanceService distanceService;

        public DistanceController(IDistanceService distanceService)
        {
            this.distanceService = distanceService;
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetDistanceBetweenAirports(string from, string to)
        {
            var airportFrom = await distanceService.GetAirportInfoAsync(from);
            var airportTo = await distanceService.GetAirportInfoAsync(to);

            if (airportFrom == null || airportTo == null)
                return NotFound("One or both iata codes are invalid.");

            var distance = distanceService.CalculateDistance(airportFrom.Location, airportTo.Location);
            return Ok(new { DistanceInMiles = distance });
        }
    }
}