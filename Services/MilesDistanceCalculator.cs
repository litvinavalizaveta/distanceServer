using DistanceServer.Models;
using GeoCoordinatePortable;

namespace DistanceServer.Services
{
    public interface IDistanceCalculator
    {
        double CalculateDistanceInMiles(Location location1, Location location2);
    }
    public class MilesDistanceCalculator : IDistanceCalculator
    {
        public double CalculateDistanceInMiles(Location location1, Location location2)
        {
            var coordinate1 = new GeoCoordinate(location1.Lat, location1.Lon);
            var coordinate2 = new GeoCoordinate(location2.Lat, location2.Lon);
            const double metersInMile = 1609.34;
            return coordinate1.GetDistanceTo(coordinate2) / metersInMile;
        }
    }
}