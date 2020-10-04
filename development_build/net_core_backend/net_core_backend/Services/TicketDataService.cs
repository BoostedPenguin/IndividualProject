using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
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
        private readonly string authId;

        public TicketDataService(ContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            authId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async override Task<SupportTicket> Get(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                return await _context.SupportTicket
                    .Where(x => x.Id == id && x.User.Auth == authId)
                    .Include(x => x.TicketChat)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<SupportTicket>> GetAllUserTickets()
        {
            using(var _context = contextFactory.CreateDbContext())
            {
                return await _context.SupportTicket
                    .Where(x => x.User.Auth == authId)
                    .Include(x => x.TicketChat)
                    .ToListAsync();
            }
        }

        public async Task<SupportTicket> CreateMessage(int ticket_id, TicketChat chat)
        {
            using(var _context = contextFactory.CreateDbContext())
            {
                var ticket = await _context.SupportTicket.Where(x => x.Id == ticket_id && x.User.Auth == authId).FirstOrDefaultAsync();

                if (ticket == null) return null;

                ticket.TicketChat.Add(chat);

                await _context.SaveChangesAsync();

                return ticket;
            }
        }
    }
}
