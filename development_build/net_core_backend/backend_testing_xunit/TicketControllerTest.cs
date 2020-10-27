using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Controllers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using net_core_backend.Context;
using net_core_backend.Services;
using System.Security.Principal;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend_testing_xunit
{
    public class TicketControllerTest : DatabaseSeeder
    {
        private ITicketService service;
        private TicketController controller;


        public TicketControllerTest(IHttpContextAccessor http, IContextFactory factory) : base(http, factory)
        {
            //Configure identity
            CreateIdentity(Users[0].Auth);
        }

        protected override void CreateIdentity(string auth)
        {
            // Configure identity
            base.CreateIdentity(auth);

            // Inject

            service = new TicketDataService(factory, http);
            controller = new TicketController(service, null)
            {
                ControllerContext = controllerContext,
            };
        }

        [Fact]
        public async Task CreateTicket()
        {
            // Inject
            CreateIdentity(Users[0].Auth);

            // Arrange
            var ticket = new SupportTicket() { Title = "Gosho", Description = "pesho", UserId = Users[0].Id };

            // Act
            var result = await controller.CreateTicket(ticket);

            // Assert
            Assert.Equal(Serialize(ticket), Serialize(((OkObjectResult)result).Value));
        }


        [Fact]
        public async Task CreateTicketAdmin()
        {
            // Inject
            CreateIdentity(Users[1].Auth);

            // Arrange
            var ticket = new SupportTicket() { Title = "Gosho", Description = "pesho", UserId = Users[1].Id, };

            // Act
            var result = await controller.CreateTicket(ticket);

            // Assert
            Assert.Equal("Administrators cannot create tickets!", ((BadRequestObjectResult)result).Value);
        }


        [Fact]
        public async Task CreateMessage()
        {
            // Inject 
            CreateIdentity(Users[0].Auth);

            // Arrange
            var ticket = new SupportTicket() { Title = "Gosho", Description = "pesho", UserId = Users[0].Id, };
            TicketChat message;

            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(ticket);
                await a.SaveChangesAsync();

                message = new TicketChat() { TicketId = ticket.Id, Message = "Some message" };

                await a.AddAsync(message);
                await a.SaveChangesAsync();
                ticket = await a.SupportTicket.Include(x => x.User).Where(x => x.Id == ticket.Id).FirstOrDefaultAsync();
            }

            // Act
            var result = await controller.CreateMessage(ticket.Id, message);

            // Assert
            Assert.Equal(Serialize(message), Serialize(((OkObjectResult)result).Value));
        }



        [Fact]
        public async Task GetTicket()
        {
            // Inject 
            CreateIdentity(Users[0].Auth);

            // Arrange
            SupportTicket addedTicket;
            using(var a = factory.CreateDbContext())
            {
                await a.AddAsync(new SupportTicket() { Title = "Testing get", Description = "Testing get description", UserId = Users[0].Id });
                await a.AddAsync(new SupportTicket() { Title = "rawr", Description = "zaw", UserId = Users[0].Id });
                await a.SaveChangesAsync();

                addedTicket = await a.SupportTicket.Where(x => x.Title == "Testing get" && x.Description == "Testing get description").Include(x => x.TicketChat).Include(x => x.User).FirstOrDefaultAsync();
            }

            // Act
            var result = await controller.GetTicket(addedTicket.Id);

            // Assert
            Assert.Equal(Serialize(addedTicket), Serialize(((OkObjectResult)result).Value));
        }


        [Fact]
        public async Task GetTicketAdmin()
        {
            // Inject
            CreateIdentity(Users[1].Auth);

            // Arrange
            SupportTicket addedTicket;
            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(new SupportTicket() { Title = "Testing get", Description = "Testing get description", UserId = Users[0].Id });
                await a.SaveChangesAsync();

                addedTicket = await a.SupportTicket.Where(x => x.Title == "Testing get" && x.Description == "Testing get description").Include(x => x.TicketChat).Include(x => x.User).FirstOrDefaultAsync();
            }

            // Act
            var result = await controller.GetTicket(addedTicket.Id);

            // Assert
            Assert.Equal(Serialize(addedTicket), Serialize(((OkObjectResult)result).Value));
        }

        [Fact]
        public async Task GetAllUserTickets()
        {
            // Inject 
            CreateIdentity(Users[0].Auth);

            // Arrange
            List<SupportTicket> tickets;
            using(var a = factory.CreateDbContext())
            {
                tickets = await a.SupportTicket
                                    .Include(x => x.TicketChat)
                                    .Where(x => x.User.Auth == Users[0].Auth)
                                    .ToListAsync();
            }

            // Act
            var result = await controller.GetAllUserTickets();

            // Assert
            Assert.Equal(Serialize(tickets), Serialize(((OkObjectResult)result).Value));
        }
    }
}
