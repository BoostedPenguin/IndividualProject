using net_core_backend.Models;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public interface ISearchDataService
    {
        Task<GoogleDataObject> SearchForLocation(string location, string type = null);
    }
}