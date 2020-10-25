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
        private IHttpContextAccessor http;
        private ITicketService service;
        private readonly IContextFactory factory;
        private TicketController controller;


        public TicketControllerTest(IHttpContextAccessor http, IContextFactory factory) : base(factory)
        {
            //Configure identity

            this.factory = factory;
            this.http = http;

            CreateIdentity("George");
        }

        private void CreateIdentity(string auth)
        {
            var identity = new GenericIdentity(auth, ClaimTypes.NameIdentifier);
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
            Assert.Equal(ticket, ((CreatedAtActionResult)result).Value);
        }

        [Fact]
        public async Task CreateTicketAdmin()
        {
            // Inject
            CreateIdentity("SecondAuth");

            // Arrange
            var ticket = new SupportTicket() { Title = "Gosho", Description = "pesho", UserId = 2, };

            // Act
            var result = await controller.CreateTicket(ticket);

            // Assert
            Assert.Equal("Administrators cannot create tickets!", ((BadRequestObjectResult)result).Value);
        }



        [Fact]
        public async Task GetTicket()
        {
            SupportTicket addedTicket;
            using(var a = factory.CreateDbContext())
            {
                await a.AddAsync(new SupportTicket() { Title = "Testing get", Description = "Testing get description", UserId = 1 });
                await a.SaveChangesAsync();

                addedTicket = await a.SupportTicket.Where(x => x.Title == "Testing get" && x.Description == "Testing get description").Include(x => x.TicketChat).Include(x => x.User).FirstOrDefaultAsync();
            }
            

            // Act
            var result = await controller.GetTicket(addedTicket.Id);

            // Assert
            Assert.Equal(Serialize(addedTicket), Serialize(result.Value));
        }

        [Fact]
        public async Task GetTickets()
        {
            List<SupportTicket> tickets;
            using(var a = factory.CreateDbContext())
            {
                tickets = await a.SupportTicket
                                    .Include(x => x.TicketChat)
                                    .Where(x => x.User.Auth == Users[0].Auth)
                                    .ToListAsync();
            }

            // Act
            var result = await controller.GetTickets();

            // Assert
            Assert.Equal(Serialize(tickets), Serialize(result.Value));
        }

        private string Serialize(object entity)
        {
            return JsonConvert.SerializeObject(entity, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });
        }
    }
}
