using DistanceServer.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace DistanceServer.Services
{
    public interface IDistanceService
    {
        Task<Airport> GetAirportInfoAsync(string iataCode);
        double CalculateDistance(Location location1, Location location2);
    }
    public class DistanceService : IDistanceService
    {
        private readonly HttpClient _httpClient;

        public DistanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Airport> GetAirportInfoAsync(string iataCode)
        {
            var response = await _httpClient.GetStringAsync($"https://places-dev.cteleport.com/airports/{iataCode}");
            return JsonConvert.DeserializeObject<Airport>(response);
        }

        public double CalculateDistance(Location location1, Location location2)
        {
            var coordinate1 = new GeoCoordinate(location1.Lat, location1.Lon);
            var coordinate2 = new GeoCoordinate(location2.Lat, location2.Lon);
            return coordinate1.GetDistanceTo(coordinate2);
        }
    }
}