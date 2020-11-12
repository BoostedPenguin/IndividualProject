﻿using AutoMapper;
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
    public class UserControllerTest : DatabaseSeeder
    {
        private IAccountService service;
        private UserController controller;
        private readonly IMapper mapper;

        public UserControllerTest(IHttpContextAccessor http, IContextFactory factory, IMapper mapper) : base(http, factory)
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

            service = new AccountDataService(factory, http);
            controller = new UserController(service, null, mapper)
            {
                ControllerContext = controllerContext,
            };
        }

        [Fact]
        public async Task UpdateAddress()
        {
            // Arrange
            var user = new Users() { Auth = "VeryAverageAuth", City = "Burgas", Country = "BG" };

            // Inject
            CreateIdentity(user.Auth);

            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(user);
                await a.SaveChangesAsync();
            }

            user.City = "Eindhoven";
            user.Country = "NL";

            // Act
            var result = await controller.UpdateAddress(user);

            // Data won't be consistant so it should be removed anyway
            (((Users)((OkObjectResult)result).Value).UpdatedAt) = null;
            user.UpdatedAt = null;

            // Assert
            Assert.Equal(Serialize(user), Serialize(((OkObjectResult)result).Value));
        }


        [Fact]
        public async Task GetUserInfo()
        {
            // Arrange
            var user = new Users() { Auth = "NewAuth", City = "Burgas", Country = "BG" };

            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(user);
                await a.SaveChangesAsync();

                var trip = new UserTrips() { Name = "To Paris", Transportation = Transportation.Bus, UserId = user.Id };

                var ticket = new SupportTicket() { UserId = user.Id, Title = "Something", Description = "Some desc" };
                await a.AddAsync(ticket);
                await a.AddAsync(trip);

                UserKeywords[] keywords = new UserKeywords[3];
                keywords[0] = new UserKeywords() { Keyword = "Google", UserId = user.Id };
                keywords[1] = new UserKeywords() { Keyword = "FB", UserId = user.Id };
                keywords[2] = new UserKeywords() { Keyword = "PR", UserId = user.Id };
                await a.AddRangeAsync(keywords);
                await a.SaveChangesAsync();
            }

            var expected = mapper.Map<UsersViewModel>(user);

            // Inject
            CreateIdentity(user.Auth);

            // Act
            var result = await controller.GetUserInfo(user.Id);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task AddKeyword()
        {
            // Inject
            var user = new Users() { Auth = "AddKeyword", City = "Burgas", Country = "BG" };
            CreateIdentity(user.Auth);


            // Arrange
            var keywords = new List<UserKeywords>()
            {
                new UserKeywords() { Keyword = "Paris", UserId = user.Id}, 
                new UserKeywords() { Keyword = "BG", UserId = user.Id},
                new UserKeywords() { Keyword = "Somewhere where I definitely Want to go", UserId = user.Id},
                new UserKeywords() { Keyword = "MaybeImDumbAndCant Seperate the words", UserId = user.Id},
            };

            using(var a = factory.CreateDbContext())
            {
                await a.AddRangeAsync(keywords);
                await a.SaveChangesAsync();
            }

            // Act
            // Assert
        }
    }
}
