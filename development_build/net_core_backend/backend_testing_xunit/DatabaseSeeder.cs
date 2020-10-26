using net_core_backend.Models;
using net_core_backend.Context;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_testing_xunit
{
    public abstract class DatabaseSeeder
    {
        protected Users[] Users { get; private set; }
        protected SupportTicket[] SupportTickets { get; private set; }
        protected TicketChat[] SupportChat { get; private set; }


        protected IHttpContextAccessor http;
        protected ControllerContext controllerContext;
        protected readonly IContextFactory factory;


        protected DatabaseSeeder(IHttpContextAccessor http, IContextFactory factory)
        {
            this.factory = factory;
            this.http = http;

            Seed(factory);
        }

        protected virtual void CreateIdentity(string auth)
        {
            // Configure identity
            var identity = new GenericIdentity(auth, ClaimTypes.NameIdentifier);
            var contextUser = new ClaimsPrincipal(identity); //add claims as needed
            var httpContext = new DefaultHttpContext()
            {
                User = contextUser
            };

            controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            http.HttpContext = httpContext;
        }


        private void Seed(IContextFactory factory)
        {
            using (var a = factory.CreateDbContext())
            {

                // Re-creates database
                a.Database.EnsureDeleted();

                a.Database.EnsureCreated();


                // Seeds users
                Users = new Users[3]
                {
                new Users() {Auth = "George"},
                new Users() {Auth = "SecondAuth", Role = Role.Admin},
                new Users() {Auth = "ThirdAuth"},
                };

                a.AddRange(Users);
                a.SaveChanges();


                // Seeds tickets
                SupportTickets = new SupportTicket[3]
                {
                new SupportTicket() {UserId = 1, Title = "First title", Description = "First description"},
                new SupportTicket() {UserId = 2, Title = "Second title", Description = "Second description"},
                new SupportTicket() {UserId = 3, Title = "Third title", Description = "Third description"},
                };

                a.AddRange(SupportTickets);
                a.SaveChanges();


                // Seeds ticket chats
                SupportChat = new TicketChat[3]
                {
                new TicketChat() {TicketId = 1, CreatorId = 1, Message = "Created by FirstAuth for First Title ticket"},
                new TicketChat() {TicketId = 2, CreatorId = 2, Message = "Created by SecondAuth for Second Title ticket"},
                new TicketChat() {TicketId = 3, CreatorId = 3, Message = "Created by ThirdAuth for Third Title ticket"},
                };

                a.AddRange(SupportChat);
                a.SaveChanges();
            }
        }


        protected string Serialize(object entity)
        {
            return JsonConvert.SerializeObject(entity, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });
        }
    }
}
