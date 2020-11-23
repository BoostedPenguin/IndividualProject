using net_core_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static net_core_backend.Services.GoogleDataService;

namespace net_core_backend.Services
{
    public interface ISuggestionService
    {
        Task<GooglePlaceObject[]> Main(string type = null);
    }
}