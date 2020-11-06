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
        Task<Locations> AddLocation(Locations location);
        Task<Locations> RemoveLocation(int trip_id, int location_id);
        Task<UserTrips> CreateTrip();
    }
}
