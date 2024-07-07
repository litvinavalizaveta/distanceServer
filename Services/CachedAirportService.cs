using DistanceServer.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DistanceServer.Services
{
    public interface IAirportService
    {
        Task<Airport> GetAirportInfo(string iataCode);
    }
    public class CachedAirportService : IAirportService
    {
        private readonly HttpClient httpClient;
        private readonly IDistributedCache cache;

        public CachedAirportService(HttpClient httpClient, IDistributedCache cache)
        {
            this.httpClient = httpClient;
            this.cache = cache;
        }
        public async Task<Airport> GetAirportInfo(string iataCode)
        {
            var cachedData = await cache.GetStringAsync(iataCode);
            if (!string.IsNullOrEmpty(cachedData))
                return JsonConvert.DeserializeObject<Airport>(cachedData);

            var response = await httpClient.GetStringAsync($"https://places-dev.cteleport.com/airports/{iataCode}");
            var airport = JsonConvert.DeserializeObject<Airport>(response);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            await cache.SetStringAsync(iataCode, response, cacheOptions);

            return airport;
        }
    }
}
