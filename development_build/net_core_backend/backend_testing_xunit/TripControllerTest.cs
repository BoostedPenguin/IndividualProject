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
            UserTrips[] trips = new UserTrips[2];
            trips[0] = new UserTrips() { Distance = 50, Duration = 12, Name = "trip to bg", Transportation = Transportation.Car, UserId = Users[0].Id };
            trips[1] = new UserTrips() { Distance = 510, Duration = 122, Name = "trip to en", Transportation = Transportation.Bus, UserId = Users[0].Id };
            using(var a = factory.CreateDbContext())
            {
                await a.AddRangeAsync(trips);
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.GetUserTrips();

            // Assert
            Assert.Equal(Serialize(trips), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task GetTrip()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            UserTrips[] trips = new UserTrips[2];
            trips[0] = new UserTrips() { Distance = 50, Duration = 12, Name = "trip to bg", Transportation = Transportation.Car, UserId = Users[0].Id };
            trips[1] = new UserTrips() { Distance = 510, Duration = 122, Name = "trip to en", Transportation = Transportation.Bus, UserId = Users[0].Id };
            using (var a = factory.CreateDbContext())
            {
                await a.AddRangeAsync(trips);
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.GetTrip(trips[0].Id);

            // Assert
            Assert.Equal(Serialize(trips[0]), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task DeleteTrip()
        {
            // Inject
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public async Task AddLocation()
        {
            // Inject
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public async Task RemoveLocation()
        {
            // Inject
            // Arrange
            // Act
            // Assert
        }
    }
}
