using net_core_backend.Models;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public interface ISearchDataService
    {
        Task<UserKeywords[]> AddKeyword(string location, string type = null);
        Task ClearKeywords();
        Task<GoogleDataService.GooglePlaceObject[]> GetGuestSuggestions(UserKeywords keywords);
        Task<UserKeywords[]> GetKeywords();
        Task<GoogleDataObject> GetPlaceByID(string placeId);
        Task<GoogleDataService.GooglePlaceObject[]> GetSuggestions();
        Task<UserKeywords[]> RemoveKeyword(int id);
        Task<GoogleDataObject> SearchForLocation(string location, string type = null);
    }
}