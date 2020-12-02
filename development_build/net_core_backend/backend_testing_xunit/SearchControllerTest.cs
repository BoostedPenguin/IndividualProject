using AutoMapper;
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
    public class SearchControllerTest : DatabaseSeeder
    {
        private ISearchDataService service;
        private IGoogleService googleService;
        private SearchController controller;
        private ISuggestionService suggestionService;
        private readonly IMapper mapper;

        public SearchControllerTest(IHttpContextAccessor http, IContextFactory factory, IMapper mapper, ISearchDataService service, IGoogleService googleService, ISuggestionService suggestionService) : base(http, factory)
        {
            //Configure identity
            this.mapper = mapper;
            this.service = service;
            this.googleService = googleService;

            CreateIdentity(Users[0].Auth);
            this.suggestionService = suggestionService;
        }

        protected override void CreateIdentity(string auth)
        {
            // Configure identity
            base.CreateIdentity(auth);

            // Inject

            service = new SearchDataService(http, googleService, factory, suggestionService);
            controller = new SearchController(null, service, mapper)
            {
                ControllerContext = controllerContext,
            };
        }


        [Fact]
        public async Task SearchForLocation()
        {
            // Inject
            // Arrange
            var expected = new GoogleDataObject() { City = "Burgas", Country = "Bulgaria", CountryCode = "BG", Latitude = 42.5138584, Longitude = 27.469502, Types = { "establishment", "point_of_interest", "school" }, PlaceId = "ChIJc-mFqIaUpkARt2OCHhOpfk4" };
            // Act
            //var types = new string[2] { "airport", "park" };
            var result = await service.SearchForLocation("НЕГ Гьоте");

            // Assert
            Assert.Equal(Serialize(expected), Serialize(result));
        }
    }
}
