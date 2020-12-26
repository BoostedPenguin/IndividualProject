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
        Task<UserTrips> CreateTrip();
        Task<GoogleDataService.GoogleDirectionsObject[]> GetWaypointsFromWishlist();
        Task<WishListDataService.SimpleLocation[]> GetSimpleWishlistLocations();
    }
}
