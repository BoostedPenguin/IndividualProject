using Microsoft.AspNetCore.Http;
using net_core_backend.Context;
using net_core_backend.Controllers;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            CreateIdentity(base.Users[0].Auth);
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
            //var trip = new UserTrips() { Distance = 50, Duration = 12, Name = "trip to bg", Transporation =}
            // Act
            // Assert
        }

        [Fact]
        public async Task GetTrip()
        {
            // Inject
            // Arrange
            // Act
            // Assert
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
