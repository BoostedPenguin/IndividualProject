using net_core_backend.Models;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public interface ISearchDataService
    {
        Task<GoogleDataService.GooglePlaceObject[]> GetGuestSuggestions(UserKeywords keywords);
        Task<GoogleDataObject> GetPlaceByID(string placeId);
        Task<GoogleDataService.GooglePlaceObject[]> GetSuggestions();
        Task<GoogleDataObject> SearchForLocation(string location, string type = null);
    }
}