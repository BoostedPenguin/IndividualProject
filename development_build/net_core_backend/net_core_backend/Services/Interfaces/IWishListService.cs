using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IWishListService
    {
        Task<WishList> GetWishlist();
        Task<WishList> ClearWishlist();
        Task<WishList> AddLocation(Locations location);
        Task<WishList> RemoveLocation(int location_id);
        Task<GoogleDataService.GoogleDirectionsObject[]> GetWaypointsFromWishlist();
        Task<WishListDataService.SimpleLocation[]> GetSimpleWishlistLocations();
        Task SetOriginDestination(int locationId, string od);
        Task<bool> CheckOriginDestination();
        Task<UserTrips> CreateTrip(string name, string transportation);
    }
}
