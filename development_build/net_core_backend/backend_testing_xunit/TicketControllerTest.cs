using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using net_core_backend.Controllers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Services;
using System.Collections.Generic;
using System.Security.Principal;
using net_core_backend.Services.Extensions;

namespace backend_testing_xunit
{
    public class TicketControllerTest
    {
        private readonly ITicketService service;
        private readonly TicketController controller;


        public TicketControllerTest(IHttpContextAccessor http, IContextFactory factory)
        {
            //Configure identity

            var identity = new GenericIdentity("George", ClaimTypes.NameIdentifier);
            var contextUser = new ClaimsPrincipal(identity); //add claims as needed
            var httpContext = new DefaultHttpContext()
            {
                User = contextUser
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            http.HttpContext = httpContext;



            //Inject

            service = new TicketDataService(factory, http);
            controller = new TicketController(service, null)
            {
                ControllerContext = controllerContext,
            };
        }


        [Fact]
        public async Task CreateTicket()
        {
            // Arrange
            var ticket = new SupportTicket() { Title = "Gosho", Description = "pesho", UserId = 1 };

            // Act
            var result = await controller.CreateTicket(ticket);

            // Assert
            Assert.Equal(ticket.Title, result.Value.Title);
        }



        [Fact]
        public async Task GetTickets()
        {
            // Arrange
            await controller.CreateTicket(new SupportTicket() { Title = "First", Description = "FirstDesc" });
            await controller.CreateTicket(new SupportTicket() { Title = "Second", Description = "SecondDesc" });
            await controller.CreateTicket(new SupportTicket() { Title = "Third", Description = "ThirdDesc" });

            // Act
            var result = await controller.GetTicket(11);

            // Assert
            Assert.Equal("Second", result.Value.Title);
        }
    }
}
