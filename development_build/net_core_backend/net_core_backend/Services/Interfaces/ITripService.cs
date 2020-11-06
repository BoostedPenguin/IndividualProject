using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ITripService
    {
        Task<List<UserTrips>> GetUserTrips();
        Task<UserTrips> GetTrip(int id);
        Task<UserTrips> DeleteTrip(int trip_id);
        Task<Locations> AddLocation(int trip_id, Locations location);
        Task<Locations> RemoveLocation(int trip_id, int location_id);
    }
}
