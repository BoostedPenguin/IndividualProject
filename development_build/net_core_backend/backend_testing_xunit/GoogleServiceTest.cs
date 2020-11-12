using Microsoft.AspNetCore.Http;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend_testing_xunit
{
    public class GoogleServiceTest : DatabaseSeeder
    {
        private readonly IGoogleService service;
        private readonly IHttpContextAccessor http;
        private readonly IContextFactory factory;

        public GoogleServiceTest(IGoogleService service, IHttpContextAccessor http, IContextFactory factory) : base(http, factory)
        {
            this.service = service;
            this.http = http;
            this.factory = factory;
        }

        [Fact]
        public async Task CoordinatesFromLocation()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { Latitude = 42.504792599999988, Longitude = 27.4626361 };

            // Act
            var result = await service.CoordinatesFromLocation("Eiffel Tower");

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }

        [Fact]
        public async Task DistanceDurationBetweenLocations()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { Distance = "130 km", Duration = "2 hours 8 mins" };


            // Act
            var result = await service.DistanceDurationBetweenLocations("Burgas", "Varna", Transportation.Car);
            
            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }

        [Fact]
        public async Task DistanceDurationBetweenLocationsCoordinates()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { Distance = "130 km", Duration = "2 hours 8 mins" };

            var loc1 = new GoogleDataObject() { Latitude = 42.504792599999988, Longitude = 27.4626361 };
            var loc2 = new GoogleDataObject() { Latitude = 43.2140504, Longitude = 27.9147333 };


            // Act
            var result = await service.DistanceDurationBetweenLocations(loc1, loc2, Transportation.Car);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }
    }
}
