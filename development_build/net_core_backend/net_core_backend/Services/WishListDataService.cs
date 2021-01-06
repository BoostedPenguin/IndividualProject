using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static net_core_backend.Services.GoogleDataService;

namespace net_core_backend.Services
{
    public class WishListDataService : DataService<WishList>, IWishListService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly IGoogleService googleService;

        public WishListDataService(IContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor, IGoogleService googleService) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            httpContext = httpContextAccessor;
            this.googleService = googleService;
        }

        public async Task<GoogleDirectionsObject[]> GetWaypointsFromWishlist()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using(var a = contextFactory.CreateDbContext())
            {
                var wishlist = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();
                
                if (wishlist == null) throw new ArgumentException("Something went wrong. User doesn't have a wishlist");

                var waypoints = await googleService.DirectionsServiceTest("Moscow", "Sofia", wishlist.Locations.Select(x => x.PlaceId).ToArray());

                return waypoints;
            }
        }

        public async Task<bool> CheckOriginDestination()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishlist = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (wishlist == null) throw new ArgumentException("Something went wrong. User doesn't have a wishlist");

                var origin = wishlist.Locations.Any(x => x.Origin_Destination == "ORIGIN");
                
                var destination = wishlist.Locations.Any(x => x.Origin_Destination == "DESTINATION");

                if (origin && destination) return true;

                throw new ArgumentException("You need to have an origin and destination location before you proceed!");
            }
        }

        public async Task SetOriginDestination(int locationId, string od)
        {
            if (od != "ORIGIN" && od != "DESTINATION") throw new ArgumentException("Origin or destination weren't in the correct format");

            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishlist = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                var location = wishlist.Locations.Where(x => x.Id == locationId).First();


                //Checks if there is already a location assigned as an origin or destination and removes it
                var locationWithSameStatus = wishlist.Locations.Where(x => x.Origin_Destination == od).FirstOrDefault();
                
                if (locationWithSameStatus != null)
                {
                    locationWithSameStatus.Origin_Destination = null;
                }

                location.Origin_Destination = od;

                await a.SaveChangesAsync();
            }
        }

        public async Task<WishList> AddLocation(Locations location)
        {
            if (location.Lang == 0 || location.Long == 0 || location.Name == null || location.PlaceId == null) throw new ArgumentException("The location data was empty");

            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var duplicate = await a.Locations.Where(x => x.PlaceId == location.PlaceId && x.TripId == null).FirstOrDefaultAsync();
                
                if (duplicate != null) throw new ArgumentException("This location is already in your wishlist");

                var wishList = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (wishList == null) throw new ArgumentException("Something went wrong! This user doesn't have a wishlist!");


                //Assign values to make a unique location
                location.WishlistId = wishList.Id;
                location.TripId = null;


                wishList.Locations.Add(location);

                await a.SaveChangesAsync();

                return wishList;
            }
        }

        public async Task<WishList> ClearWishlist()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishList = await a.WishList.Include(x => x.Locations).Include(x => x.User).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (wishList == null) throw new ArgumentException("Something went wrong! This user doesn't have a wishlist!");

                a.RemoveRange(wishList.Locations);

                await a.SaveChangesAsync();

                return wishList;
            }
        }

        public async Task<UserTrips> CreateTrip(string name, string transportation)
        {
            if (name == null) throw new ArgumentException("The name of the tirp can't be empty!");

            transportation.CheckTransportation();
            
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var currentAuth = httpContext.GetCurrentAuth();

                var duplicateName = await a.UserTrips.Include(x => x.User).Where(x => x.Name == name && x.User.Auth == currentAuth).FirstOrDefaultAsync();

                if (duplicateName != null) throw new ArgumentException("There is already a trip with that name. Please choose another.");

                var wishList = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == currentAuth).FirstOrDefaultAsync();

                if (wishList == null) throw new ArgumentException("Something went wrong! This user doesn't have a wishlist!");

                if (wishList.Locations.Count < 2) throw new ArgumentException("You must have at least 2 locations in your wishlist!");

                var trip = new UserTrips() { Transportation = transportation, Name = name, UserId = wishList.UserId };
                
                await a.AddAsync(trip);

                await a.SaveChangesAsync();

                foreach (var l in wishList.Locations)
                {
                    l.TripId = trip.Id;
                    l.WishlistId = null;
                }
                await a.SaveChangesAsync();

                return trip;
            }
        }

        public async Task<WishList> GetWishlist()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot interact with their own wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                return await a.WishList
                    .Include(x => x.Locations)
                    .Where(x => x.User.Auth == httpContext.GetCurrentAuth())
                    .FirstOrDefaultAsync();
            }
        }

        public class SimpleLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Origin_Destination { get; set; }

        }

        public async Task<SimpleLocation[]> GetSimpleWishlistLocations()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot interact with their own wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishlist = await a.WishList
                    .Include(x => x.Locations)
                    .Where(x => x.User.Auth == httpContext.GetCurrentAuth())
                    .FirstOrDefaultAsync();

                List<SimpleLocation> sl = new List<SimpleLocation>();
                foreach(var b in wishlist.Locations)
                {
                    sl.Add(new SimpleLocation() { Latitude = b.Lang, Longitude = b.Long});
                }
                return sl.ToArray();
            }
        }

        public async Task<WishList> RemoveLocation(int location_id)
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot interact with their own wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishList = await a.WishList.Include(x => x.User).Include(x => x.Locations).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (wishList == null) throw new ArgumentException("Something went wrong! This user doesn't have a wishlist!");

                var location = await a.Locations.Where(x => x.Id == location_id && x.WishlistId == wishList.Id && x.TripId == null).FirstOrDefaultAsync();

                a.Remove(location);

                await a.SaveChangesAsync();

                return wishList;
            }
        }
    }
}
