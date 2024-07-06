using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DistanceServer.Services;

namespace DistanceServer.Controllers
{
    [Route("api/distance")]
    [ApiController]
    public class DistanceController : Controller
    {
        private readonly IDistanceService distanceService;

        public DistanceController(IDistanceService distanceService)
        {
            this.distanceService = distanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistanceBetweenAirports(string from, string to)
        {
            var airportFrom = await distanceService.GetAirportInfoAsync(from);
            var airportTo = await distanceService.GetAirportInfoAsync(to);

            if (airportFrom == null || airportTo == null)
                return NotFound("One or both iata codes are invalid.");

            const double metersInMile = 1609.34;
            var distance = distanceService.CalculateDistanceInMeters(airportFrom.Location, airportTo.Location) / metersInMile;
            return Ok(new { DistanceInMiles = distance });
        }
    }
}