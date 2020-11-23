using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleDataObject> CoordinatesFromLocation(string location);
        Task<GoogleDataObject> DistanceDurationBetweenLocations(string location1, string location2, Transportation transportation);
        Task<GoogleDataObject> DistanceDurationBetweenLocations(GoogleDataObject latLngLocation1, GoogleDataObject latLngLocation2, Transportation transportation);
        Task<List<GoogleDataService.GooglePlaceObject>> GetNearbyPlaces(UserKeywords input, string type = null, int radius = 10000);
        Task<GoogleDataObject> LocationFromCoordinates(GoogleDataObject coordinates);
        Task<GoogleDataObject> LocationFromLandmark(string landmark);
    }
}
