using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class WishListControllerTest : DatabaseSeeder
    {
        private IWishListService service;
        private WishListController controller;

        public WishListControllerTest(IHttpContextAccessor http, IContextFactory factory) : base(http, factory)
        {
            //Configure identity
            CreateIdentity(Users[0].Auth);
        }

        protected override void CreateIdentity(string auth)
        {
            // Configure identity
            base.CreateIdentity(auth);

            // Inject

            service = new WishListDataService(factory, http);
            controller = new WishListController(service, null)
            {
                ControllerContext = controllerContext,
            };
        }

        [Fact]
        public async Task GetWishlist()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            var expected = WishLists[0];
            expected.User = null;

            // Act
            var result = await controller.GetWishlist();

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task ClearWishlist()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            var expected = WishLists[0];
            expected.User = null;

            using(var a = factory.CreateDbContext())
            {
                WishLists[0].Locations.Add(new Locations() { Lang = 12, Long = 31, Name = "kdfvc", TripId = null, WishlistId = WishLists[0].Id });
                WishLists[0].Locations.Add(new Locations() { Lang = 55, Long = 321, Name = "zawsga", TripId = null, WishlistId = WishLists[0].Id });
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.ClearWishlist();

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }
    }
}
