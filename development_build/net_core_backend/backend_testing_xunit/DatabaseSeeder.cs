using net_core_backend.Models;
using net_core_backend.Context;



namespace backend_testing_xunit
{
    public abstract class DatabaseSeeder
    {
        protected Users[] Users { get; private set; }
        protected SupportTicket[] SupportTickets { get; private set; }
        protected TicketChat[] SupportChat { get; private set; }

        protected DatabaseSeeder(IContextFactory factory)
        {
            Seed(factory);
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
    }
}
