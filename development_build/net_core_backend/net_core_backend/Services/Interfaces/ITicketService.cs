using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketChat> CreateMessage(int ticket_id, TicketChat chat);
        Task<SupportTicket> CreateTicket(SupportTicket entity);
        Task<List<SupportTicket>> GetAllUserTickets();
        Task<SupportTicket> GetTicket(int id);
    }
}
