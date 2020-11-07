using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Controllers;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend_testing_xunit
{
    public class TripControllerTest : DatabaseSeeder
    {
        private ITripService service;
        private TripController controller;

        public TripControllerTest(IHttpContextAccessor http, IContextFactory factory) : base(http, factory)
        {
            //Configure identity
            CreateIdentity(Users[0].Auth);
        }

        protected override void CreateIdentity(string auth)
        {
            // Configure identity
            base.CreateIdentity(auth);

            // Inject

            service = new TripDataService(factory, http);
            controller = new TripController(service, null)
            {
                ControllerContext = controllerContext,
            };
        }

        [Fact]
        public async Task GetUserTrips()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            var expected = UserTrips.Where(x => x.UserId == Users[0].Id).Reverse().ToList();
            foreach(var e in expected)
            {
                e.User = null;
            }

            // Act
            var result = await controller.GetUserTrips();

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task GetTrip()
        {
            // Inject
            CreateIdentity(Users[0].Auth);


            // Act
            var result = await controller.GetTrip(UserTrips[0].Id);

            // Assert
            Assert.Equal(Serialize(UserTrips[0]), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task DeleteTrip()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            UserTrips trip;
            using(var a = factory.CreateDbContext())
            {
                trip = new UserTrips() { UserId = Users[0].Id, Name = "Trip to somewhere", Transportation = Transportation.Bus };
                trip.Locations.Add(new Locations() { WishlistId = null, TripId = trip.Id, Name = "kfg", Lang = 5, Long = 3 });
                trip.Locations.Add(new Locations() { WishlistId = null, TripId = trip.Id, Name = "aweawe", Lang = 15, Long = 33 });

                await a.AddAsync(trip);
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.DeleteTrip(trip.Id);

            // Assert
            Assert.Equal(Serialize(trip), Serialize(((OkObjectResult)result).Value));

        }

        [Fact]
        public async Task AddLocation()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            UserTrips trip;
            Locations locations;
            using (var a = factory.CreateDbContext())
            {
                trip = new UserTrips() { UserId = Users[0].Id, Name = "Trip to somewhere 2", Transportation = Transportation.Bus };

                locations = new Locations() { Lang = 5, Long = 3, Name = "aweawezsS", TripId = trip.Id };
                trip.Locations.Add(locations);

                await a.AddAsync(trip);
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.AddLocation(trip.Id, locations);

            // Assert
            Assert.Equal(Serialize(locations), Serialize(((OkObjectResult)result).Value));

        }

        [Fact]
        public async Task RemoveLocation()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            UserTrips trip;
            Locations locations;
            using (var a = factory.CreateDbContext())
            {
                trip = new UserTrips() { UserId = Users[0].Id, Name = "Trip to somewhere 2", Transportation = Transportation.Bus };

                locations = new Locations() { Lang = 5, Long = 3, Name = "aweawezsS", TripId = trip.Id };
                trip.Locations.Add(locations);

                await a.AddAsync(trip);
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.RemoveLocation(trip.Id, locations.Id);

            // Assert
            Assert.Equal(Serialize(locations), Serialize(((OkObjectResult)result).Value));

        }
    }
}
