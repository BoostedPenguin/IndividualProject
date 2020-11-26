using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public interface IWikipediaDataService
    {
        Task<string> GetWikipediaPage(string info = null);
    }
}