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
        Task<GoogleDataObject> LocationFromCoordinates(GoogleDataObject coordinates);
        Task<GoogleDataObject> LocationFromLandmark(string landmark);
        Task NearbyPlaceShift(GoogleDataObject input, UserKeywords keyword);
    }
}
