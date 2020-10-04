using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ITicketService : IDataService<SupportTicket>
    {
        Task<SupportTicket> CreateMessage(int ticket_id, TicketChat chat);
        Task<IEnumerable<SupportTicket>> GetAllUserTickets();
    }
}
