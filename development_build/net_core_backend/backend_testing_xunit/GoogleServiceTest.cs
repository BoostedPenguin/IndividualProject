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
            var result = await service.CoordinatesFromLocation("Burgas");

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }


        [Fact]
        public async Task LocationFromCoordinates()
        {
            // Inject
            // Arrange
            var coordinates = new GoogleDataObject() { Latitude = 42.513361, Longitude = 27.465091 };
            var expected = new GoogleDataObject() { City = "Burgas", Country = "Bulgaria", Latitude = 42.5133555, Longitude = 27.4650935, CountryCode = "BG", Types = { "establishment", "health", "hospital", "point_of_interest" } };
            // Act
            var result = await service.LocationFromCoordinates(coordinates);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }

        [Fact]
        public async Task LocationFromLandmark()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { City = "Burgas", Country = "Bulgaria", CountryCode = "BG", Latitude = 42.5138584, Longitude = 27.469502, Types = { "establishment", "point_of_interest", "school"} };
            // Act
            var result = await service.LocationFromLandmark("NEG \"Goethe \"");

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }

        [Fact]
        public async Task DistanceDurationBetweenLocations()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { Distance = "130 km", Duration = "2 hours 4 mins" };


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
            var expected = new GoogleDataObject() { Distance = "130 km", Duration = "2 hours 4 mins" };

            var loc1 = new GoogleDataObject() { Latitude = 42.504792599999988, Longitude = 27.4626361 };
            var loc2 = new GoogleDataObject() { Latitude = 43.2140504, Longitude = 27.9147333 };


            // Act
            var result = await service.DistanceDurationBetweenLocations(loc1, loc2, Transportation.Car);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }

        [Fact]
        public async Task Testing()
        {
            // Inject

            // Act
            //await service.NearbyPlaceShift(new GoogleDataObject() { Latitude = 42.513361, Longitude = 27.465091, MainType = "school" });

        }
    }
}
