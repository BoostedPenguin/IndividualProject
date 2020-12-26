using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static net_core_backend.Services.GoogleDataService;

namespace net_core_backend.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleDataObject> CoordinatesFromLocation(string location);
        Task<GoogleDirectionsObject[]> DirectionsServiceTest(string origin, string destination, string[] locations);
        Task DistanceBetweenMultipleLocations(string origin, string[] destination);
        Task<GoogleDataObject> DistanceDurationBetweenLocations(string location1, string location2, Transportation transportation);
        Task<GoogleDataObject> DistanceDurationBetweenLocations(GoogleDataObject latLngLocation1, GoogleDataObject latLngLocation2, Transportation transportation);
        Task<GoogleDataObject> GetLocationFromPlaceID(string placeId);
        Task<List<GoogleDataService.GooglePlaceObject>> GetNearbyPlaces(UserKeywords input, string type = null, int radius = 25000, string searchKeyword = "attraction");
        Task<GoogleDataObject> LocationFromCoordinates(GoogleDataObject coordinates);
        Task<GoogleDataObject> LocationFromLandmark(string landmark, string[] givenType = null);
    }
}
