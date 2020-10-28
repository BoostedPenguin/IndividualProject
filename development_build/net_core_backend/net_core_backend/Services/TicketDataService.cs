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
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;

        public TicketDataService(IContextFactory _contextFactory, IHttpContextAccessor httpContextAccessor) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            httpContext = httpContextAccessor;
        }

        public async Task<SupportTicket> GetTicket(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var ticket = await _context.SupportTicket
                    .Where(x => x.Id == id)
                    .Include(x => x.TicketChat)
                    .FirstOrDefaultAsync();

                if (CurrentExtensions.HasPrivileges(ticket.UserId, httpContext, contextFactory)) return ticket;

                throw new ArgumentException("Access forbidden!");
            }
        }

        public async Task<List<SupportTicket>> GetAllUserTickets()
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                return await _context.SupportTicket
                    .Where(x => x.User.Auth == httpContext.GetCurrentAuth())
                    .Include(x => x.TicketChat)
                    .ToListAsync();
            }
        }

        public async Task<TicketChat> CreateMessage(int ticket_id, TicketChat chat)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var ticket = await _context.SupportTicket.Include(x => x.User).Where(x => x.Id == ticket_id && x.User.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if (ticket == null) return null;

                ticket.TicketChat.Add(chat);

                await _context.SaveChangesAsync();

                return chat;
            }
        }

        public async Task<SupportTicket> CreateTicket(SupportTicket entity)
        {
            if(await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot create tickets!");
            }

            var id = await base.GetUserId(httpContext.GetCurrentAuth());

            entity.UserId = id;

            return await base.Create(entity);
        }
    }
}
