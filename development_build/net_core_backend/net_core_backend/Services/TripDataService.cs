﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class TripDataService : DataService<UserTrips>, ITripService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;

        public TripDataService(IContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            httpContext = httpContextAccessor;
        }

        public async Task<Locations> AddLocation(int trip_id, Locations location)
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot add locations!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var trip = await a.UserTrips.Where(x => x.Id == trip_id && x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (trip == null) throw new ArgumentException("There isn't a trip with that id!");

                //Assign values to make a unique location
                location.WishlistId = null;
                location.TripId = trip.Id;


                trip.Locations.Add(location);

                await a.SaveChangesAsync();

                return location;
            }
        }
        public async Task<Locations> RemoveLocation(int trip_id, int location_id)
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot create tickets!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var trip = await a.UserTrips.Where(x => x.Id == trip_id).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (trip == null) throw new ArgumentException("There isn't a trip with that id!");

                var location = await a.Locations.Where(x => x.Id == location_id && x.WishlistId == null && x.TripId == trip_id).FirstOrDefaultAsync();

                a.Remove(location);

                return location;
            }
        }

        public async Task<UserTrips> DeleteTrip(int trip_id)
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot create tickets!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var trip = await a.UserTrips.Where(x => x.Id == trip_id && x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (trip == null) throw new ArgumentException("There isn't a trip with that id!");

                var locations = await a.Locations.Where(x => x.TripId == trip.Id && x.WishlistId == null).ToListAsync();
                
                a.RemoveRange(locations);
                a.Remove(trip);

                return trip;
            }
        }

        public async Task<UserTrips> GetTrip(int id)
        {
            using(var a = contextFactory.CreateDbContext())
            {
                var trip = await a.UserTrips
                    .Include(x => x.Locations)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();


                if (CurrentExtensions.HasPrivileges(trip.UserId, httpContext, contextFactory)) return trip;

                throw new ArgumentException("Access forbidden!");
            }
        }

        public async Task<List<UserTrips>> GetUserTrips()
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot interact with their own trips!");
            }

            using (var a = contextFactory.CreateDbContext())
            {
                var z = await a.UserTrips
                    .Include(x => x.Locations)
                    .Include(x => x.User)
                    .Where(x => x.User.Auth == httpContext.GetCurrentAuth())
                    .ToListAsync();

                return z;
            }
        }
    }
}
