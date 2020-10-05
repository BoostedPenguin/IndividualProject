using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using net_core_backend.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class TicketDataService : DataService<SupportTicket>, ITicketService
    {
        private readonly ContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;

        public TicketDataService(ContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            httpContext = httpContextAccessor;
        }

        public async override Task<SupportTicket> Get(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                return await _context.SupportTicket
                    .Where(x => x.Id == id && x.User.Auth == httpContext.GetCurrentAuth())
                    .Include(x => x.TicketChat)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<SupportTicket>> GetAllUserTickets()
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                return await _context.SupportTicket
                    .Where(x => x.User.Auth == httpContext.GetCurrentAuth())
                    .Include(x => x.TicketChat)
                    .ToListAsync();
            }
        }

        public async Task<SupportTicket> CreateMessage(int ticket_id, TicketChat chat)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var ticket = await _context.SupportTicket.Where(x => x.Id == ticket_id && x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (ticket == null) return null;

                ticket.TicketChat.Add(chat);

                await _context.SaveChangesAsync();

                return ticket;
            }
        }

        public async override Task<SupportTicket> Create(SupportTicket entity)
        {
            var id = await base.GetUserId(httpContext.GetCurrentAuth());
            entity.UserId = id;

            return await base.Create(entity);
        }
    }
}
