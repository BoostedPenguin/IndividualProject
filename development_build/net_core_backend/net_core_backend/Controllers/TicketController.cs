using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly ITicketService _context;

        public TicketController(ITicketService context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<SupportTicket>> GetTicket(int id)
        {
            try
            {
                var a = await _context.GetTicket(id);
                return Ok(a);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<SupportTicket>> GetTickets()
        {
            try
            {
                return Ok(await _context.GetAllUserTickets());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/message")]
        [Authorize]
        public async Task<ActionResult<SupportTicket>> CreateMessage(int ticket_id, TicketChat chat)
        {
            try
            {
                return Ok(await _context.CreateMessage(ticket_id, chat));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SupportTicket>> CreateTicket(SupportTicket ticket)
        {
            try
            {
                return Ok(await _context.CreateTicket(ticket));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
