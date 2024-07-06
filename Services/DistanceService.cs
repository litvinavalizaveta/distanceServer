using DistanceServer.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DistanceServer.Services
{
    public interface IDistanceService
    {
        Task<Airport> GetAirportInfoAsync(string iataCode);
        double CalculateDistanceInMeters(Location location1, Location location2);
    }
    public class DistanceService : IDistanceService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public DistanceService(HttpClient httpClient, IDistributedCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<Airport> GetAirportInfoAsync(string iataCode)
        {
            var cachedData = await _cache.GetStringAsync(iataCode);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<Airport>(cachedData);

            var response = await _httpClient.GetStringAsync($"https://places-dev.cteleport.com/airports/{iataCode}");
            var airport = JsonConvert.DeserializeObject<Airport>(response);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            await _cache.SetStringAsync(iataCode, response, cacheOptions);

            return airport;
        }

        public double CalculateDistanceInMeters(Location location1, Location location2)
        {
            var coordinate1 = new GeoCoordinate(location1.Lat, location1.Lon);
            var coordinate2 = new GeoCoordinate(location2.Lat, location2.Lon);
            return coordinate1.GetDistanceTo(coordinate2);
        }
    }
}