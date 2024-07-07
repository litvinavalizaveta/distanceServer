using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DistanceServer.Services;

namespace DistanceServer.Controllers
{
    [Route("api/distance")]
    [ApiController]
    public class DistanceController : Controller
    {
        private readonly IDistanceCalculator distanceService;
        private readonly IAirportService airportService;
        
        public DistanceController(IDistanceCalculator distanceService, IAirportService airportService)
        {
            this.distanceService = distanceService;
            this.airportService = airportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistanceBetweenAirports(string from, string to)
        {
            var airportFrom = await airportService.GetAirportInfo(from);
            var airportTo = await airportService.GetAirportInfo(to);

            if (airportFrom == null || airportTo == null)
                return NotFound(new { Error = "One or both iata codes are invalid." });

            var distance = distanceService.CalculateDistanceInMiles(airportFrom.Location, airportTo.Location);
            return Ok(new { DistanceInMiles = distance });
        }
    }
}