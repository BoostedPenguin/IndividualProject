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
        static protected Users[] Users { get; private set; }
        static protected WishList[] WishLists { get; private set; }
        static protected SupportTicket[] SupportTickets { get; private set; }
        static protected TicketChat[] SupportChat { get; private set; }
        static protected UserTrips[] UserTrips { get; private set; }
        static protected Locations[] Locations { get; private set; }


        protected IHttpContextAccessor http;
        protected ControllerContext controllerContext;
        protected readonly IContextFactory factory;


        protected DatabaseSeeder(IHttpContextAccessor http, IContextFactory factory)
        {
            this.factory = factory;
            this.http = http;

            //Seed(factory);
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


        public static void Seed(IContextFactory factory)
        {
            using (var a = factory.CreateDbContext())
            {

                // Re-creates database
                a.Database.EnsureDeleted();
                a.Database.EnsureCreated();


                // Seeds users
                Users = new Users[4]
                {
                new Users() {Auth = "George"},
                new Users() {Auth = "SecondAuth", Role = Role.Admin},
                new Users() {Auth = "ThirdAuth"},
                new Users() {Auth = "auth0|5f955e7b9b8822006ee06870", Name = "RealAccount"},
                };

                a.AddRange(Users);
                a.SaveChanges();


                // Seeds wishlist
                WishLists = new WishList[3]
                {
                    new WishList() {Transportation = Transportation.Bus, UserId = Users[0].Id},
                    new WishList() {Transportation = Transportation.Car, UserId = Users[1].Id},
                    new WishList() {Transportation = Transportation.Walk, UserId = Users[2].Id},
                };

                a.AddRange(WishLists);
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

                UserTrips = new UserTrips[3]
                {
                new UserTrips() { Distance = 50, Duration = 12, Name = "trip to bg", Transportation = Transportation.Car, UserId = Users[0].Id },
                new UserTrips() { Distance = 510, Duration = 122, Name = "trip to en", Transportation = Transportation.Bus, UserId = Users[0].Id },
                new UserTrips() { Distance = 540, Duration = 1222, Name = "trip to nl", Transportation = Transportation.Walk, UserId = Users[1].Id },
                };


                a.AddRange(UserTrips);
                a.SaveChanges();

                Locations = new Locations[5]
                {
                new Locations() { Lang = 5, Long = 3, Name = "BS", TripId = UserTrips[0].Id },
                new Locations() { Lang = 5, Long = 3, Name = "zaw", TripId = UserTrips[0].Id },
                new Locations() { Lang = 5, Long = 3, Name = "awesda", WishlistId = WishLists[0].Id },
                new Locations() { Lang = 5, Long = 53, Name = "zafg", WishlistId = WishLists[0].Id },
                new Locations() { Lang = 5, Long = 3, Name = "hjm", TripId = UserTrips[2].Id },
                };


                //Because entity is retarded and inverses order if i dont do this
                a.Add(Locations[0]);
                a.SaveChanges();
                a.Add(Locations[1]);
                a.SaveChanges();
                a.Add(Locations[2]);
                a.SaveChanges();
                a.Add(Locations[3]);
                a.SaveChanges();
                a.Add(Locations[4]);
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
