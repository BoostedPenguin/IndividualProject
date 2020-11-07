using AutoMapper;
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
    public class WishListControllerTest : DatabaseSeeder
    {
        private IWishListService service;
        private WishListController controller;
        private readonly IMapper mapper;

        public WishListControllerTest(IHttpContextAccessor http, IContextFactory factory, IMapper mapper) : base(http, factory)
        {
            //Configure identity
            CreateIdentity(Users[0].Auth);
            this.mapper = mapper;
        }

        protected override void CreateIdentity(string auth)
        {
            // Configure identity
            base.CreateIdentity(auth);

            // Inject

            service = new WishListDataService(factory, http);
            controller = new WishListController(service, null, mapper)
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
            WishList list;
            using(var a = factory.CreateDbContext())
            {
                list = await a.WishList.Include(x => x.Locations).Where(x => x.Id == WishLists[0].Id).FirstOrDefaultAsync();
            }
            var expected = mapper.Map<WishListViewModel>(list);

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
            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(new Locations() { Lang = 12, Long = 31, Name = "kdfvc", TripId = null, WishlistId = WishLists[0].Id });
                await a.AddAsync(new Locations() { Lang = 55, Long = 321, Name = "zawsga", TripId = null, WishlistId = WishLists[0].Id });
                await a.SaveChangesAsync();
            }

            // Act
            var result = await controller.ClearWishlist();

            WishListViewModel expected;

            using(var a = factory.CreateDbContext())
            {
                var z = await a.WishList.Include(x => x.Locations).Where(x => x.UserId == Users[0].Id).FirstOrDefaultAsync();
                
                expected = mapper.Map<WishListViewModel>(z);
            }


            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }
    }
}
