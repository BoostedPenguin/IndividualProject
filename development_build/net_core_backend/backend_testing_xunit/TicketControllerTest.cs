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
using AutoMapper;
using System.Security.Cryptography;

namespace backend_testing_xunit
{
    public class TicketControllerTest : DatabaseSeeder
    {
        private ITicketService service;
        private TicketController controller;
        private readonly IMapper mapper;

        public TicketControllerTest(IHttpContextAccessor http, IContextFactory factory, IMapper mapper) : base(http, factory)
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

            service = new TicketDataService(factory, http);
            controller = new TicketController(service, null, mapper)
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

            SupportTicket exp;
            // Act
            var result = await controller.CreateTicket(ticket);

            using (var a = factory.CreateDbContext())
            {
                exp = await a.SupportTicket.Where(x => x.Title == "Gosho" && x.Description == "pesho").FirstOrDefaultAsync();
            }

            var expected = mapper.Map<SupportTicketViewModel>(exp);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
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

            var expected = mapper.Map<TicketChatViewModel>(message);

            // Act
            var result = await controller.CreateMessage(ticket.Id, message);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }



        [Fact]
        public async Task GetTicket()
        {
            // Inject 
            CreateIdentity(Users[0].Auth);

            // Arrange
            SupportTicket addedTicket = new SupportTicket() { Title = "Testing get", Description = "Testing get description", UserId = Users[0].Id };
            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(addedTicket);
                await a.AddAsync(new SupportTicket() { Title = "rawr", Description = "zaw", UserId = Users[0].Id });
                await a.SaveChangesAsync();
            }

            var expected = mapper.Map<SupportTicketViewModel>(addedTicket);

            // Act
            var result = await controller.GetTicket(addedTicket.Id);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }


        [Fact]
        public async Task GetTicketAdmin()
        {
            // Inject
            CreateIdentity(Users[1].Auth);

            // Arrange
            SupportTicket addedTicket = new SupportTicket() { Title = "Testing get", Description = "Testing get description", UserId = Users[0].Id };
            SupportTicket addedTicket2 = new SupportTicket() { Title = "Testing get 2", Description = "Testing get description 2", UserId = Users[0].Id };
            using (var a = factory.CreateDbContext())
            {
                await a.AddAsync(addedTicket);
                await a.AddAsync(addedTicket2);
                await a.SaveChangesAsync();
            }

            var expected = mapper.Map<SupportTicketViewModel>(addedTicket);

            // Act
            var result = await controller.GetTicket(addedTicket.Id);

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
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

            var expected = mapper.Map<List<SupportTicketViewModel>>(tickets);

            // Act
            var result = await controller.GetAllUserTickets();

            // Assert
            Assert.Equal(Serialize(expected), Serialize(((OkObjectResult)result).Value));
        }
    }
}
