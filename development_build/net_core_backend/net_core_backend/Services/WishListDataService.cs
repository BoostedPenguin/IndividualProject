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

namespace net_core_backend.Services
{
    public class WishListDataService : DataService<WishList>, IWishListService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;

        public WishListDataService(IContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            httpContext = httpContextAccessor;
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

        public async Task<UserTrips> CreateTrip()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot intact with their wishlist!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var wishList = await a.WishList.Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (wishList == null) throw new ArgumentException("Something went wrong! This user doesn't have a wishlist!");

                var locations = await a.Locations.Where(x => x.TripId == null && x.WishlistId == wishList.Id).ToListAsync();

                if (locations.Count == 0) throw new ArgumentException("You must have at least 1 location in your wishlist!");

                var trip = new UserTrips() { Distance = wishList.Distance, Duration = wishList.Duration, Transportation = wishList.Transportation, Name = wishList.Name, UserId = wishList.UserId };
                foreach (var l in locations)
                {
                    l.TripId = trip.Id;
                    l.WishlistId = null;
                }

                await a.AddAsync(trip);
                await a.AddRangeAsync(locations);

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
